using AutoMapper;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<GetEventByIdQuery> _validator;

    public GetEventByIdQueryHandler(IEventRepository eventRepository, ILogger<GetEventByIdQueryHandler> logger,
        IMapper mapper, IValidator<GetEventByIdQuery> validator)
    {
        _eventRepository = eventRepository;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<EventResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var eventId = Guid.Parse(request.Id);
        var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (@event is null)
        {
            _logger.LogWarning("Event with id {EventId} not found", eventId);
            throw new EventNotFoundException($"Event with id {eventId} not found");
        }

        var eventResponse = _mapper.Map<EventResponse>(@event);

        return eventResponse;
    }
}