using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(IEventRepository eventRepository, ILogger<GetEventByIdQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<EventResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var eventId = Guid.Parse(request.Id);
        var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (@event == null)
        {
            _logger.LogWarning("Event with id {EventId} not found", eventId);
            throw new NullReferenceException();
        }

        return new EventResponse
        (@event.Id.ToString(), @event.Name, @event.Description, @event.StartDate, @event.EndDate, @event.Location,
            @event.Category.ToString(), @event.Capacity,
            @event.Images.Select(i => new ImageResponse(i.Id.ToString(), i.ImageUrl)));
    }
}