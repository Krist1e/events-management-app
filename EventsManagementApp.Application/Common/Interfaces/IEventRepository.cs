using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Event @event, CancellationToken cancellationToken);
    
    Task<Event?> GetEventByNameAsync(string eventName, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsByDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsByLocationAsync(string location, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsByCategoryAsync(CategoryEnum category, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}