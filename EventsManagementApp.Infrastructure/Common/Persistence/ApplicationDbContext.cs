using EventManagementApp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Common.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(u => u.ToTable("Users"));
        modelBuilder.Entity<IdentityUserClaim<Guid>>(uc => uc.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<Guid>>(ul => ul.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<Guid>>(ut => ut.ToTable("UserTokens"));
        
        modelBuilder.Entity<Role>(r => r.ToTable("Roles"));
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(rc => rc.ToTable("RoleClaims"));
        
        modelBuilder.Entity<IdentityUserRole<Guid>>(ur => ur.ToTable("UserRoles"));
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
}