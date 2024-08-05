using Microsoft.AspNetCore.Identity;

namespace EventManagementApp.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Role Role { get; set; }
    public Guid RoleId { get; set; }
}