using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEvents;

public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;

    public ListEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventResponse>> Handle(ListEventsQuery request,
        CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);

        var eventResponses = events.Select(e =>
            new EventResponse(
                e.Id.ToString(),
                e.Name,
                e.Description,
                e.StartDate,
                e.EndDate,
                e.Location,
                e.Category.ToString(),
                e.Capacity,
                e.Images.Select(i => new ImageResponse(i.Id.ToString(), i.ImageUrl))
                )
        );

        return eventResponses;
    }
}