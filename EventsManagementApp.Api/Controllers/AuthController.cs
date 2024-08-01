using Asp.Versioning;
using EventsManagementApp.Application.UseCases.Auth.Login;
using EventsManagementApp.Application.UseCases.Auth.Logout;
using EventsManagementApp.Application.UseCases.Auth.RefreshToken;
using EventsManagementApp.Application.UseCases.Auth.Register;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = EventsManagementApp.Application.UseCases.Users.Contracts.LoginRequest;
using RegisterRequest = EventsManagementApp.Application.UseCases.Users.Contracts.RegisterRequest;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registration,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new RegisterCommand(registration), cancellationToken);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login, CancellationToken cancellationToken)
    {
        await _sender.Send(new LoginCommand(login), cancellationToken);

        return Empty;
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest, CancellationToken cancellationToken)
    {
        var newPrincipal = await _sender.Send(new RefreshTokenCommand(refreshRequest), cancellationToken);

        return SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _sender.Send(new LogoutCommand(), cancellationToken);
        
        return Ok();
    }
}