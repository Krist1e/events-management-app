using EventsManagementApp.Application.Common.Constants;
using EventsManagementApp.Application.Common.Mappings;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.Validators.Common;
using EventsManagementApp.Application.Validators.Events;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventsManagementApp.Application.Common;

public static class Dependencies
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy("RequireUserRole", policy => policy.RequireRole(Roles.User));
        });
        
        services.AddScoped<IOptionsMonitor<BearerTokenOptions>, OptionsMonitor<BearerTokenOptions>>();

        services.AddValidatorsFromAssemblyContaining<CreateEventCommandValidator>(filter:
            t => t.ValidatorType.Namespace?.Contains(typeof(GuidValidator).Namespace!) == false);

        services.AddAutoMapper(typeof(EventProfile).Assembly);

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<CreateEventCommandHandler>();
        });
    }
}