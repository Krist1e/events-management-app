using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public record CreateEventCommand(EventRequest Event): IRequest<EventResponse>;