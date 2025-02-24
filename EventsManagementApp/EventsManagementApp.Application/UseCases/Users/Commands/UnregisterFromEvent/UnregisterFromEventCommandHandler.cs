﻿using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;

public class UnregisterFromEventCommandHandler : IRequestHandler<UnregisterFromEventCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnregisterFromEventCommandHandler> _logger;
    private readonly IValidator<UnregisterFromEventCommand> _validator;

    public UnregisterFromEventCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ILogger<UnregisterFromEventCommandHandler> logger, IValidator<UnregisterFromEventCommand> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task Handle(UnregisterFromEventCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var userId = Guid.Parse(request.UserId);
        var eventId = Guid.Parse(request.EventId);

        var result = await _userRepository.RemoveUserFromEventAsync(userId, eventId, cancellationToken);

        if (!result)
        {
            _logger.LogWarning("User with id {UserId} is not registered for event with id {EventId}", userId, eventId);
            throw new UnregisterFromEventFailedException(
                $"Failed to unregister user with id {userId} from event with id {eventId}");
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("User with id {UserId} unregistered from event with id {EventId}", userId, eventId);
    }
}