﻿using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<LoginCommandHandler> _logger; 

    public LoginCommandHandler(SignInManager<User> signInManager, ILogger<LoginCommandHandler> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(request.User.Email, request.User.Password,
            isPersistent: false,
            lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to log in user with email {email}", request.User.Email);
            throw new LoginFailedException($"Failed to log in user with email {request.User.Email}");
        }
        
        _logger.LogInformation("User logged in: {email}", request.User.Email);
    }
}