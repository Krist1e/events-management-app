using EventManagementApp.Domain.Entities;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventsManagementApp.Common.Extensions;

public static class CollectionsExtension
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
        });

        return services;
    }
}