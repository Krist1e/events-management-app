using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Common.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>, IUnitOfWork
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
    public DbSet<EventImage> EventImages { get; set; }

    public async Task CommitChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackChangesAsync(CancellationToken cancellationToken)
    {
        ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        await Task.CompletedTask;
    }
}