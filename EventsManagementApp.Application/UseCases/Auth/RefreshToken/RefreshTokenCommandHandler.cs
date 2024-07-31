using System.Security.Claims;
using EventManagementApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventsManagementApp.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ClaimsPrincipal>
{
    private readonly IOptionsMonitor<BearerTokenOptions> _optionsMonitor;
    private readonly TimeProvider _timeProvider = TimeProvider.System;
    private readonly SignInManager<User> _signInManager;
    private ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(IOptionsMonitor<BearerTokenOptions> optionsMonitor,
        SignInManager<User> signInManager, ILogger<RefreshTokenCommandHandler> logger)
    {
        _optionsMonitor = optionsMonitor;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<ClaimsPrincipal> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenProtector = _optionsMonitor.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(request.Refresh.RefreshToken);

        var refreshTokenExpired = refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc ||
                                  _timeProvider.GetUtcNow() >= expiresUtc;

        var user = await _signInManager.ValidateSecurityStampAsync(refreshTicket?.Principal);
        var securityTimestampValidationFailed = user is null;

        if (refreshTokenExpired ||
            securityTimestampValidationFailed)
        {
            _logger.LogError("Failed to refresh token");
            throw new RefreshTokenException("Failed to refresh token");
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        _logger.LogInformation("Token refreshed for user: {email}", user!.Email);
        return newPrincipal;
    }
}