using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartWebScraper.API.Controllers;
using SmartWebScraper.Application.Features.Commands.SaveSearchResult;
using SmartWebScraper.Persistence;
using System;

namespace SmartWebScraper.API;

public static class StartupExtensions
{
    public static void ApplyDbMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Applying database migrations...");
                var dbContext = services.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
    }

    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies
        (typeof(SaveSearchResultCommand).Assembly, typeof(SearchController).Assembly));
        return services;
    }
}
