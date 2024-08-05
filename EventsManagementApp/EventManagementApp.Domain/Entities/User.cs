using Microsoft.AspNetCore.Identity;

namespace EventManagementApp.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; } 
    public ICollection<UserRole> UserRoles { get; set; } 
}