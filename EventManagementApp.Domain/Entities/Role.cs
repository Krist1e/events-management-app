using Microsoft.AspNetCore.Identity;

namespace EventManagementApp.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}