using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Users.Persistence;

public class UserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.HasData(
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            }
        );
        
        builder.ToTable("UserRoles");
    }
}