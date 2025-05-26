using Microsoft.EntityFrameworkCore;
using SmartWebScraper.Domain;
using SmartWebScraper.Persistence.Configurations;

namespace SmartWebScraper.Persistence;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SearchResult> SearchResults { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SearchResultConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);

    }
}
