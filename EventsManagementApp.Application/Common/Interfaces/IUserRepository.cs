using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(User user, CancellationToken cancellationToken);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    
    Task<bool> AddUserToEventAsync(UserEvent userEvent, CancellationToken cancellationToken);
    Task<bool> RemoveUserFromEventAsync(UserEvent userEvent, CancellationToken cancellationToken);
}