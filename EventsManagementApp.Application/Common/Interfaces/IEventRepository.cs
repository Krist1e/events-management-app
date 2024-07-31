using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Event @event, CancellationToken cancellationToken);

    Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}