using AutoMapper;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;

public class ListEventsByUserIdQueryHandler : IRequestHandler<ListEventsByUserIdQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public ListEventsByUserIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventResponse>> Handle(ListEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var events = await _eventRepository.GetEventsByUserIdAsync(userId, cancellationToken);

        var eventResponses = _mapper.Map<IEnumerable<EventResponse>>(events);

        return eventResponses;
    }
}