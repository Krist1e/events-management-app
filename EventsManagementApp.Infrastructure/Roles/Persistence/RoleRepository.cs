using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Roles.Persistence;

public class RoleRepository : IRoleRepository
{
    private readonly RoleStore<Role, ApplicationDbContext, Guid> _roleStore;

    public RoleRepository(RoleStore<Role, ApplicationDbContext, Guid> roleStore)
    {
        _roleStore = roleStore;
        _roleStore.AutoSaveChanges = false;
    }

    public async Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        return await _roleStore.FindByIdAsync(roleId.ToString(), cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _roleStore.Roles.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(Role role, CancellationToken cancellationToken)
    {
        await _roleStore.CreateAsync(role, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var existingRole = await GetByIdAsync(role.Id, cancellationToken);

        if (existingRole is null)
        {
            return false;
        }

        await _roleStore.UpdateAsync(role, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        var existingRole = await GetByIdAsync(role.Id, cancellationToken);

        if (existingRole is null)
        {
            return false;
        }

        await _roleStore.DeleteAsync(role, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var roles = await _roleStore.Context.Roles
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _roleStore.Context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
    }

    public async Task<bool> AddRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken)
    {
        var existingUser = await _roleStore.Context.Users
            .FirstOrDefaultAsync(u => u.Id == userRole.UserId, cancellationToken);

        var existingRole = await GetByIdAsync(userRole.RoleId, cancellationToken);

        if (existingUser is null || existingRole is null) 
            return false;

        await _roleStore.Context.UserRoles.AddAsync(userRole, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken)
    {
        var existingUserRole = await _roleStore.Context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId, cancellationToken);

        if (existingUserRole is null)
            return false;
        
        _roleStore.Context.UserRoles.Remove(userRole);
        return true;
    }
}