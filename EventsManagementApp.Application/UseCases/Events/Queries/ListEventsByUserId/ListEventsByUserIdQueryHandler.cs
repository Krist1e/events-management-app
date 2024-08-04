using AutoMapper;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;

public class ListEventsByUserIdQueryHandler : IRequestHandler<ListEventsByUserIdQuery, PagedResponse<EventResponse>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ListEventsByUserIdQuery> _validator;

    public ListEventsByUserIdQueryHandler(IEventRepository eventRepository, IMapper mapper,
        IValidator<ListEventsByUserIdQuery> validator)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<PagedResponse<EventResponse>> Handle(ListEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var userId = Guid.Parse(request.UserId);
        var events = await _eventRepository.GetEventsByUserIdAsync(userId, request.QueryParameters, cancellationToken);

        var eventResponses = _mapper.Map<PagedResponse<EventResponse>>(events);

        return eventResponses;
    }
}