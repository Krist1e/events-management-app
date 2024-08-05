using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CreateEventCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEventCommand> _validator;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository,
        ILogger<CreateEventCommandHandler> logger, IMapper mapper, IValidator<CreateEventCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var newEvent = _mapper.Map<Event>(request.Event);

        var eventId = await _eventRepository.CreateAsync(newEvent, cancellationToken);

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Event with id {EventId} created", eventId);

        var eventResponse = _mapper.Map<CreateEventResponse>(newEvent);
        
        return eventResponse;
    }
}