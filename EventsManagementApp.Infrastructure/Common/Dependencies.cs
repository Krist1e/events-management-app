using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using EventsManagementApp.Infrastructure.Events.Persistence;
using EventsManagementApp.Infrastructure.Images.Persistence;
using EventsManagementApp.Infrastructure.Images.Storage;
using EventsManagementApp.Infrastructure.Users.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManagementApp.Infrastructure.Common;

public static class Dependencies
{
    public static void AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services
            .AddScoped<IRoleStore<Role>, RoleStore<Role, ApplicationDbContext, Guid, UserRole, IdentityRoleClaim<Guid>>>();
        services.AddScoped<IUserStore<User>, UserStore<User, Role, ApplicationDbContext, Guid, IdentityUserClaim<Guid>,
            UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IImageService, ImageService>();
    }
}