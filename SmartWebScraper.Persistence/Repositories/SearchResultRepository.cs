using SmartWebScraper.Application.Contracts;
using SmartWebScraper.Domain;

namespace SmartWebScraper.Persistence.Repositories;
public class SearchResultRepository(AppDbContext dbContext) : Repository<SearchResult>(dbContext), ISearchResultRepository
{
}
