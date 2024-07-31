using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;

public record UpdateEventCommand(EventRequest Event) : IRequest<EventResponse>;