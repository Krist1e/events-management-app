using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Constants;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ILogger<RegisterCommandHandler> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.User.Email;

        var user = _mapper.Map<User>(request.User);

        var result = await _userRepository.CreateAsync(user, request.User.Password, cancellationToken);

        if (!result)
        {
            _logger.LogError("Failed to create user with email {Email}", email);
            throw new RegisterFailedException("Failed to create user");
        }

        var roleAdded = await _userRepository.AddRoleToUserAsync(user.Id, Roles.User, cancellationToken);
        
        if (!roleAdded)
        {
            _logger.LogError("Failed to add role to user with email {Email}", email);
            throw new RoleAssignmentFailedException("Failed to assign role to user");
        }
        
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        _logger.LogInformation("User created: {email}", email);
    }
}