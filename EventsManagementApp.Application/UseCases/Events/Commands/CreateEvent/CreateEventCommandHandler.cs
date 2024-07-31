using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository,
        ILogger<CreateEventCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Name = request.CreateEvent.Name,
            Description = request.CreateEvent.Description,
            StartDate = request.CreateEvent.StartDate,
            EndDate = request.CreateEvent.EndDate,
            Location = request.CreateEvent.Location,
            Category = Enum.Parse<CategoryEnum>(request.CreateEvent.Category),
            Capacity = request.CreateEvent.Capacity
        };

        var eventId = await _eventRepository.CreateAsync(newEvent, cancellationToken);

        if (eventId == Guid.Empty)
        {
            throw new Exception("Failed to create event");
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Event with id {EventId} created", eventId);

        return new CreateEventResponse
        (
            eventId.ToString(),
            newEvent.Name,
            newEvent.Description,
            newEvent.StartDate,
            newEvent.EndDate,
            newEvent.Location,
            newEvent.Category.ToString(),
            newEvent.Capacity
        );
    }
}