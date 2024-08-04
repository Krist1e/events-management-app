using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.UseCases.Auth.Contracts;
using EventsManagementApp.Application.UseCases.Auth.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Auth;

public class LoginCommandTests
{
    private readonly LoginCommandHandler _handler;
    private readonly Mock<SignInManager<User>> _signInManagerMock;

    public LoginCommandTests()
    {
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null,
            null, null, null);
        _signInManagerMock = new Mock<SignInManager<User>>(userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
        
        Mock<ILogger<LoginCommandHandler>> loggerMock = new();

        _handler = new LoginCommandHandler(
            _signInManagerMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldLogInUser_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var command = new LoginCommand(new LoginRequest(user.Email, "password123"));

        _signInManagerMock.Setup(m => m.PasswordSignInAsync(user.Email, "password123", false, false))
            .ReturnsAsync(SignInResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _signInManagerMock.Verify(m => m.PasswordSignInAsync(user.Email, "password123", false, false), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowLoginFailedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var command = new LoginCommand(new LoginRequest(user.Email, "wrongpassword"));

        _signInManagerMock.Setup(m => m.PasswordSignInAsync(user.Email, "wrongpassword", false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<LoginFailedException>(() => _handler.Handle(command, CancellationToken.None));

        _signInManagerMock.Verify(m => m.PasswordSignInAsync(user.Email, "wrongpassword", false, false), Times.Once);
    }
}