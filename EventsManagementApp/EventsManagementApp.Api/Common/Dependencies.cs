using EventsManagementApp.Middleware;
using Microsoft.OpenApi.Models;

namespace EventsManagementApp.Common;

public static class Dependencies
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();
    }
    
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Management API", Version = "v1.0" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    
}