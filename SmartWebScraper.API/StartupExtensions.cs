using Microsoft.EntityFrameworkCore;
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
}
