using AutoMapper;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListFilteredEvents;

public class ListFilteredEventsQueryHandler : IRequestHandler<ListFilteredEventsQuery, PagedResponse<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ListFilteredEventsQuery> _validator;

    public ListFilteredEventsQueryHandler(IEventRepository eventRepository, IMapper mapper,
        IValidator<ListFilteredEventsQuery> validator)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<PagedResponse<EventResponse>> Handle(ListFilteredEventsQuery request,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var events = await _eventRepository.GetFilteredEventsAsync(request.QueryParameters, cancellationToken);

        var eventsResponse = _mapper.Map<PagedResponse<EventResponse>>(events);

        return eventsResponse;
    }
}