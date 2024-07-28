using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
    }

    public async Task<EventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Name = request.Event.Name,
            Description = request.Event.Description,
            StartDate = request.Event.StartDate,
            EndDate = request.Event.EndDate,
            Location = request.Event.Location,
            Category = Enum.Parse<CategoryEnum>(request.Event.Category),
            Capacity = request.Event.Capacity
        };

        var eventId = await _eventRepository.CreateAsync(newEvent, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return new EventResponse
        (
            eventId,
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