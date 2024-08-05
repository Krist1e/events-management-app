using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.UseCases.Auth.Logout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Auth;

public class LogoutCommandHandlerTests
{
    private readonly LogoutCommandHandler _handler;
    private readonly Mock<SignInManager<User>> _signInManagerMock;

    public LogoutCommandHandlerTests()
    {
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null,
            null, null, null);
        _signInManagerMock = new Mock<SignInManager<User>>(userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
        Mock<ILogger<User>> loggerMock = new();

        _handler = new LogoutCommandHandler(_signInManagerMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldLogOutUser()
    {
        // Arrange
        var command = new LogoutCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);
    }
}