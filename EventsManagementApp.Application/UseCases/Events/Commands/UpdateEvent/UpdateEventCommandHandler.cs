using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventCommandHandler> _logger;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork,
        ILogger<UpdateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<EventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Name = request.Event.Name,
            Description = request.Event.Description,
            StartDate = request.Event.StartDate,
            EndDate = request.Event.EndDate,
            Location = request.Event.Location,
            Category = Enum.Parse<CategoryEnum>(request.Event.Category),
            Capacity = request.Event.Capacity
        };

        var result = await _eventRepository.UpdateAsync(@event, cancellationToken);
        
        if (!result)
        {
            _logger.LogWarning("Event with id {EventId} not found", @event.Id);
            throw new NullReferenceException();
        }
        
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Event with id {EventId} updated", @event.Id);

        return new EventResponse
        (
            @event.Id.ToString(),
            @event.Name, @event.Description,
            @event.StartDate,
            @event.EndDate,
            @event.Location,
            @event.Category.ToString(),
            @event.Capacity,
            @event.Images.Select(i => new ImageResponse(i.Id.ToString(), i.ImageUrl))
        );
    }
}