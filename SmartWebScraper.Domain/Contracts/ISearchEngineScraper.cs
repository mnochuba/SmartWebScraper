namespace SmartWebScraper.Domain.Contracts;
public interface ISearchEngineScraper
{
    Task<Dictionary<int, string>> GetSearchResultPositionsAsync(string searchPhrase, string targetUrl);
}
