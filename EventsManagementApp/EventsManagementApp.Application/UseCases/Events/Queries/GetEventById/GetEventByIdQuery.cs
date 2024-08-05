using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;

public record GetEventByIdQuery(string Id) : IRequest<EventResponse>;