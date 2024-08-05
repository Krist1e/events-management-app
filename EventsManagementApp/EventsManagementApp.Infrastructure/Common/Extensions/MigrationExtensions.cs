using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Infrastructure.Common.Extensions;

public static class MigrationExtensions
{
    public static void AddMigrations<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddHostedService<MigrationHostedService<TContext>>();
    }

    private class MigrationHostedService<TContext> : BackgroundService
        where TContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken) =>
            MigrateDbContext(_serviceProvider);

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
            Task.CompletedTask;

        private static async Task MigrateDbContext(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var context = scopedProvider.GetRequiredService<TContext>();
            var logger = scopedProvider.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name);

                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () => await context.Database.MigrateAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name);

                throw;
            }
        }
    }
}