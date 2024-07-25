using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(Role role, CancellationToken cancellationToken);
    Task UpdateAsync(Role role, CancellationToken cancellationToken);
    Task DeleteAsync(Role role, CancellationToken cancellationToken);
    
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Role> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
    Task AddRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    Task RemoveRoleFromUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
}