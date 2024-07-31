using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListEvents;

public record ListEventsQuery : IRequest<IEnumerable<EventResponse>>;