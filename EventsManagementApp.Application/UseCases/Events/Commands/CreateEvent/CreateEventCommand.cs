using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public record CreateEventCommand(EventRequest Event): IRequest<EventResponse>;