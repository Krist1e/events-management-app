using AutoMapper;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;

public class ListEventsByUserIdQueryHandler : IRequestHandler<ListEventsByUserIdQuery, PagedResponse<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public ListEventsByUserIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<EventResponse>> Handle(ListEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var events = await _eventRepository.GetEventsByUserIdAsync(userId, request.QueryParameters, cancellationToken);

        var eventResponses = _mapper.Map<PagedResponse<EventResponse>>(events);

        return eventResponses;
    }
}