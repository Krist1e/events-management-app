using EventManagementApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Common.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserClaim<Guid>>(uc => uc.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<Guid>>(ul => ul.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<Guid>>(ut => ut.ToTable("UserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(rc => rc.ToTable("RoleClaims"));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }
    public new DbSet<UserRole> UserRoles { get; set; }
}