using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> CreateAsync(User user, string password, CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<bool> AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken);
    
    Task<bool> AddUserToEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
    Task<bool> RemoveUserFromEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
}