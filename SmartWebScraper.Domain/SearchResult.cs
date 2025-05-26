namespace SmartWebScraper.Domain;
public record SearchResult
{
    public int Id { get; init; }
    public string SearchPhrase { get; init; } = default!;
    public string TargetUrl { get; init; } = "infotrack.co.uk";
    public string Positions { get; init; } = default!;
    public string URLs { get; init; } = default!;
    public DateTime SearchDate { get; init; } = default!;

    public SearchResult(string searchPhrase, List<int> positions, List<string> urls, string targetUrl = "infotrack.co.uk")
    {
        SearchPhrase = searchPhrase;
        TargetUrl = targetUrl;
        Positions = positions.Concatenate();
        URLs = urls.Concatenate();
        SearchDate = DateTime.UtcNow;
    }

    // Required by EF Core
    private SearchResult() { }
}
