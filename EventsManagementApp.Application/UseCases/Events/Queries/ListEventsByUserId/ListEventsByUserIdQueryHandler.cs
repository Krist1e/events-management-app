using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;

public class ListEventsByUserIdQueryHandler : IRequestHandler<ListEventsByUserIdQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;

    public ListEventsByUserIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventResponse>> Handle(ListEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var events = await _eventRepository.GetEventsByUserIdAsync(userId, cancellationToken);

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