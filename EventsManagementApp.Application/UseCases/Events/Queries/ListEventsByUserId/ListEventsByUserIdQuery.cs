using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;

public record ListEventsByUserIdQuery(string UserId) : IRequest<IEnumerable<EventResponse>>;