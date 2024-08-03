using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Queries.ListFilteredEvents;

public record ListFilteredEventsQuery(EventQueryParameters QueryParameters) : IRequest<PagedResponse<EventResponse>>;