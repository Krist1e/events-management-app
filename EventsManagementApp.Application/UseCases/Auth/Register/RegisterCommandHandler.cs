using System.ComponentModel.DataAnnotations;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Constants;
using EventsManagementApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.User.Email;

        if (string.IsNullOrEmpty(email) || !EmailAddressAttribute.IsValid(email))
        {
            return false;
        }

        var user = new User
        {
            UserName = email,
            Email = email,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
            DateOfBirth = request.User.DateOfBirth
        };

        var result = await _userRepository.CreateAsync(user, request.User.Password, cancellationToken);

        if (!result)
        {
            _logger.LogError("Failed to create user with email {Email}", email);
            return false;
        }

        await _userRepository.AddRoleToUserAsync(user.Id, Roles.User, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        _logger.LogInformation("User created: {email}", email);
        
        return true;
    }
}