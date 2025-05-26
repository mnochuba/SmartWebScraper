using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartWebScraper.Persistence;
public static class PersistenceServiceRegistration
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    private static string? GetConnectionString(IConfiguration configuration)
    {
        var connectionString = "Server = localhost\\SQLEXPRESS; Database= SmartWebScrapperDb; Integrated Security = True; TrustServerCertificate=true;";
            //Environment.GetEnvironmentVariable("SeoWebScrapperConnection") ?? // For more secure handling of connection strings
            //configuration.GetConnectionString("DefaultConnection"); // Fallback to appsettings.json default connection string for testing or development
        return connectionString;
    }
}
