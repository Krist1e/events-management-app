using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(User user, CancellationToken cancellationToken);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    
    Task AddUserToEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
    Task RemoveUserFromEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
}