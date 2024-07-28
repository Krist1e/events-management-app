using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Events.Persistence;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EventRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> GetByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
        return @event;
    }

    public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events.ToListAsync(cancellationToken);
        return events;
    }

    public async Task<Guid> CreateAsync(Event @event, CancellationToken cancellationToken)
    {
        var newEvent = await _dbContext.Events.AddAsync(@event, cancellationToken);
        return newEvent.Entity.Id;
    }

    public async Task<bool> UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        var existingEvent = await GetByIdAsync(@event.Id, cancellationToken);

        if (existingEvent is null)
            return false;

        _dbContext.Events.Update(@event);
        return true;
    }

    public async Task<bool> DeleteAsync(Event @event, CancellationToken cancellationToken)
    {
        var existingEvent = await GetByIdAsync(@event.Id, cancellationToken);

        if (existingEvent is null)
            return false;

        _dbContext.Events.Remove(@event);
        return true;
    }

    public async Task<Event?> GetEventByNameAsync(string eventName, CancellationToken cancellationToken)
    {
        var @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Name == eventName, cancellationToken);
        return @event;
    }

    public async Task<IEnumerable<Event>> GetEventsByDateAsync(DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events.Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
            .ToListAsync(cancellationToken);
        return events;
    }

    public async Task<IEnumerable<Event>> GetEventsByLocationAsync(string location, CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events.Where(e => e.Location == location).ToListAsync(cancellationToken);
        return events;
    }

    public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(CategoryEnum category,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events.Where(e => e.Category == category).ToListAsync(cancellationToken);
        return events;
    }

    public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events
            .Where(e => e.UserEvents.Any(ue => ue.UserId == userId))
            .ToListAsync(cancellationToken);

        return events;
    }
}