using Microsoft.AspNetCore.Identity;

namespace EventManagementApp.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; } 
}