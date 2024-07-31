using EventsManagementApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;

public class RegisterInEventCommandHandler : IRequestHandler<RegisterInEventCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterInEventCommandHandler> _logger;

    public RegisterInEventCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ILogger<RegisterInEventCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RegisterInEventCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var eventId = Guid.Parse(request.EventId);

        var result = await _userRepository.AddUserToEventAsync(userId, eventId, cancellationToken);

        if (!result)
        {
            _logger.LogWarning("User with id {UserId} couldn't be registered for event with id {EventId}", userId,
                eventId);
            return false;
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("User with id {UserId} registered for event with id {EventId}", userId, eventId);

        return true;
    }
}