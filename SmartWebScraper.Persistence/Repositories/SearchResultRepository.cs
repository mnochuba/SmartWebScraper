using SmartWebScraper.Domain.Contracts;
using SmartWebScraper.Domain;

namespace SmartWebScraper.Persistence.Repositories;
public class SearchResultRepository(AppDbContext dbContext) : Repository<SearchResult>(dbContext), ISearchResultRepository
{
}
