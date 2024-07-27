using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Users.Persistence;

public class UserRepository : IUserRepository
{
    private readonly UserStore<User, Role, ApplicationDbContext, Guid> _userStore;

    public UserRepository(UserStore<User, Role, ApplicationDbContext, Guid> userStore)
    {
        _userStore = userStore;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userStore.FindByIdAsync(userId.ToString(), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _userStore.Users.ToListAsync(cancellationToken);
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
        var user = await _userStore.Context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var users = await _userStore.Context.Users
            .Where(u => u.UserEvents.Any(ue => ue.EventId == eventId))
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<bool> AddUserToEventAsync(UserEvent userEvent, CancellationToken cancellationToken)
    {
        var context = _userStore.Context;
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userEvent.UserId, cancellationToken);
        var existingEvent =
            await context.Events.FirstOrDefaultAsync(e => e.Id == userEvent.EventId, cancellationToken);

        if (existingUser is null || existingEvent is null)
            return false;

        await context.UserEvents.AddAsync(userEvent, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveUserFromEventAsync(UserEvent userEvent, CancellationToken cancellationToken)
    {
        var context = _userStore.Context;
        var existingUserEvent = await context.UserEvents
            .FirstOrDefaultAsync(ue => ue.UserId == userEvent.UserId && ue.EventId == userEvent.EventId,
                cancellationToken);

        if (existingUserEvent is null)
            return false;

        context.UserEvents.Remove(userEvent);
        return true;
    }
}