using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public record CreateEventCommand(CreateEventRequest CreateEvent): IRequest<CreateEventResponse>;