using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateEventCommand> _validator;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork,
        ILogger<UpdateEventCommandHandler> logger, IMapper mapper, IValidator<UpdateEventCommand> validator)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<EventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var @event = _mapper.Map<Event>(request.Event);

        var result = await _eventRepository.UpdateAsync(@event, cancellationToken);
        
        if (!result)
        {
            _logger.LogWarning("Event with id {EventId} not found", @event.Id);
            throw new EventNotFoundException($"Event with id {@event.Id} not found");
        }
        
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Event with id {EventId} updated", @event.Id);

        var eventResponse = _mapper.Map<EventResponse>(@event);
        
        return eventResponse;
    }
}