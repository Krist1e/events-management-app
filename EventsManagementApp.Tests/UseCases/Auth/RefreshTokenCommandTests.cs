using System.Security.Claims;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.UseCases.Auth.RefreshToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Auth;

public class RefreshTokenCommandTests
{
    private readonly RefreshTokenCommandHandler _handler;
    private readonly Mock<IOptionsMonitor<BearerTokenOptions>> _optionsMonitorMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;

    public RefreshTokenCommandTests()
    {
        _optionsMonitorMock = new Mock<IOptionsMonitor<BearerTokenOptions>>();
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null,
            null, null, null);
        _signInManagerMock = new Mock<SignInManager<User>>(userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
        Mock<ILogger<RefreshTokenCommandHandler>> loggerMock = new();

        _handler = new RefreshTokenCommandHandler(
            _optionsMonitorMock.Object,
            _signInManagerMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowRefreshTokenException_WhenRefreshTokenIsExpired()
    {
        // Arrange
        var command = new RefreshTokenCommand(new RefreshRequest
        {
            RefreshToken = "expired_token"
        });

        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity()),
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(-10)
            },
            "Bearer"
        );
        var options = new BearerTokenOptions
        {
            RefreshTokenProtector =
                Mock.Of<ISecureDataFormat<AuthenticationTicket>>(p => p.Unprotect(It.IsAny<string>()) == ticket)
        };

        _optionsMonitorMock.Setup(o => o.Get(IdentityConstants.BearerScheme))
            .Returns(options);

        // Act & Assert
        await Assert.ThrowsAsync<RefreshTokenException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowRefreshTokenException_WhenSecurityStampValidationFails()
    {
        // Arrange
        var command = new RefreshTokenCommand(new RefreshRequest
        {
            RefreshToken = "invalid_token"
        });

        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity()),
            new AuthenticationProperties(),
            "Bearer"
        );
        var options = new BearerTokenOptions
        {
            RefreshTokenProtector =
                Mock.Of<ISecureDataFormat<AuthenticationTicket>>(p => p.Unprotect(It.IsAny<string>()) == ticket)
        };

        _optionsMonitorMock.Setup(o => o.Get(IdentityConstants.BearerScheme)).Returns(options);
        _signInManagerMock.Setup(s => s.ValidateSecurityStampAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<RefreshTokenException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnNewPrincipal_WhenTokenIsValid()
    {
        // Arrange
        var command = new RefreshTokenCommand(new RefreshRequest
        {
            RefreshToken = "valid_token"
        });

        var user = new User { Email = "test@example.com" };
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity()),
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            },
            "Bearer"
        );

        var options = new BearerTokenOptions
        {
            RefreshTokenProtector =
                Mock.Of<ISecureDataFormat<AuthenticationTicket>>(p => p.Unprotect(It.IsAny<string>()) == ticket)
        };

        _optionsMonitorMock.Setup(o => o.Get(IdentityConstants.BearerScheme)).Returns(options);
        _signInManagerMock.Setup(s => s.ValidateSecurityStampAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _signInManagerMock.Setup(s => s.CreateUserPrincipalAsync(user)).ReturnsAsync(ticket.Principal);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}