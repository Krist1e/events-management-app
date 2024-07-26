using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(Role role, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Role role, CancellationToken cancellationToken);
    
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
    Task<bool> AddRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken);
    Task<bool> RemoveRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken);
}