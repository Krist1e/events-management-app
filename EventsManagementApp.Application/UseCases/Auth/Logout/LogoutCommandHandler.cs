using EventManagementApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<User> _logger;

    public LogoutCommandHandler(SignInManager<User> signInManager, ILogger<User> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
    }
}