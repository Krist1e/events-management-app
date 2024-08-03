using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.UseCases.Events.Contracts;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken);
    Task<PagedResponse<Event>> GetFilteredEventsAsync(EventQueryParameters queryParameters,
        CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Event @event, CancellationToken cancellationToken);

    Task<PagedResponse<Event>> GetEventsByUserIdAsync(Guid userId,
        QueryParameters queryParameters, CancellationToken cancellationToken);
}