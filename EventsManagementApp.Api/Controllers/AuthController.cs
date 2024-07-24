using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using EventManagementApp.Contracts.Users;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Common.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IOptionsMonitor<BearerTokenOptions> _optionsMonitor;
    private readonly TimeProvider _timeProvider = TimeProvider.System;
    private readonly IUserStore<User> _userStore;
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public AuthController(ILogger<AuthController> logger, UserManager<User> userManager,
        SignInManager<User> signInManager, IOptionsMonitor<BearerTokenOptions> optionsMonitor,
        IUserStore<User> userStore)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _optionsMonitor = optionsMonitor;
        _userStore = userStore;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest registration)
    {
        var emailStore = (IUserEmailStore<User>)_userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return BadRequest("Invalid email address");
        }

        var user = new User
        {
            UserName = email,
            Email = email,
            FirstName = registration.FirstName,
            LastName = registration.LastName,
            DateOfBirth = registration.DateOfBirth
        };

        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        await _userManager.AddToRoleAsync(user, Roles.User);

        var result = await _userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to create user: {errors}", result.Errors);
            return BadRequest(result);
        }

        _logger.LogInformation("User created: {email}", email);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserRequest login, [FromQuery] bool? useCookies = false,
        [FromQuery] bool? useSessionCookies = false)
    {
        var useCookieScheme = useCookies == true || useSessionCookies == true;
        var isPersistent = useCookies == true && useSessionCookies != true;

        _signInManager.AuthenticationScheme =
            useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result =
            await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent,
                lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to login user: {email}", login.Email);
            return Unauthorized(result);
        }
        
        _logger.LogInformation("User logged in: {email}", login.Email);

        return Empty;
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = _optionsMonitor.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        var refreshTokenExpired = refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc ||
                                  _timeProvider.GetUtcNow() >= expiresUtc;

        var user = await _signInManager.ValidateSecurityStampAsync(refreshTicket?.Principal);
        var securityTimestampValidationFailed = user is null;

        if (refreshTokenExpired ||
            securityTimestampValidationFailed)
        {
            _logger.LogError("Failed to refresh token");
            return Challenge();
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        _logger.LogInformation("Token refreshed for user: {email}", user!.Email);

        return SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return Ok();
    }
}