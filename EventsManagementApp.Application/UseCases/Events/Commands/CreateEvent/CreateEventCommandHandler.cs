using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CreateEventCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository,
        ILogger<CreateEventCommandHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = _mapper.Map<Event>(request.Event);

        var eventId = await _eventRepository.CreateAsync(newEvent, cancellationToken);

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Event with id {EventId} created", eventId);

        var eventResponse = _mapper.Map<CreateEventResponse>(newEvent);
        
        return eventResponse;
    }
}