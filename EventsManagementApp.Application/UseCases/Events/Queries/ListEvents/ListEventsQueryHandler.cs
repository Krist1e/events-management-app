using AutoMapper;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEvents;

public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, IEnumerable<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public ListEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventResponse>> Handle(ListEventsQuery request,
        CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);
        var eventResponses = _mapper.Map<IEnumerable<EventResponse>>(events);
        
        return eventResponses;
    }
}