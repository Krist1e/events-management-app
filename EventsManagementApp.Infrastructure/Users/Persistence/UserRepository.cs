using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Users.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IUserStore<User> _userStore;
    private readonly ApplicationDbContext _context;

    public UserRepository(IUserStore<User> userStore, ApplicationDbContext context)
    {
        _userStore = userStore;
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userStore.FindByIdAsync(userId.ToString(), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _userStore.CreateAsync(user, cancellationToken);
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await GetByIdAsync(user.Id, cancellationToken);

        if (existingUser is null)
        {
            return false;
        }

        await _userStore.UpdateAsync(user, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await GetByIdAsync(user.Id, cancellationToken);

        if (existingUser is null)
        {
            return false;
        }

        await _userStore.DeleteAsync(user, cancellationToken);
        return true;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(u => u.UserEvents.Any(ue => ue.EventId == eventId))
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<bool> AddUserToEventAsync(Guid userId, Guid eventId, bool isOrganizer,
        CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (existingUser is null || existingEvent is null)
            return false;

        var userEvent = new UserEvent
        {
            UserId = userId,
            EventId = eventId,
            RegistrationDate = DateTime.UtcNow
        };

        await _context.UserEvents.AddAsync(userEvent, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveUserFromEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var userEvent = await _context.UserEvents
            .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

        if (userEvent is null)
            return false;

        _context.UserEvents.Remove(userEvent);
        return true;
    }
}