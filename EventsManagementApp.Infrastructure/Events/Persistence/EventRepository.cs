using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Infrastructure.Common.Extensions;
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
        var @event = await _dbContext.Events.Include(e => e.Images)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
        return @event;
    }

    public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events.Include(e => e.Images).ToListAsync(cancellationToken);
        return events;
    }

    public async Task<PagedResponse<Event>> GetFilteredEventsAsync(EventQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var events = _dbContext.Events.Include(e => e.Images)
            .Filter(queryParameters)
            .Search(queryParameters)
            .Sort(queryParameters.OrderBy);
        
        var totalCount = await events.CountAsync(cancellationToken);

        var pagedEvents = await events.Paginate(queryParameters).ToListAsync(cancellationToken);
        
        var metadata = new PaginationMetadata(totalCount, queryParameters.PageNumber, queryParameters.PageSize);
        var pagedResponse = new PagedResponse<Event>(pagedEvents, metadata);

        return pagedResponse;
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

    public async Task<PagedResponse<Event>> GetEventsByUserIdAsync(Guid userId, QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events
            .Where(e => e.UserEvents.Any(ue => ue.UserId == userId))
            .Include(e => e.Images)
            .Paginate(queryParameters)
            .ToListAsync(cancellationToken);
        
        var totalCount = events.Count;
        var metadata = new PaginationMetadata(totalCount, queryParameters.PageNumber, queryParameters.PageSize);
        var pagedResponse = new PagedResponse<Event>(events, metadata);

        return pagedResponse;
    }
}