using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWebScraper.Domain;

namespace SmartWebScraper.Persistence.Configurations;
public class SearchResultConfiguration : IEntityTypeConfiguration<SearchResult>
{
    public void Configure(EntityTypeBuilder<SearchResult> builder)
    {
        builder.ToTable("SearchResults");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.SearchPhrase).IsRequired();
        builder.Property(s => s.TargetUrl).IsRequired().HasMaxLength(300);
        builder.Property(s => s.Positions);
        builder.Property(s => s.URLs);
        builder.Property(s => s.SearchDate);
    }
}
