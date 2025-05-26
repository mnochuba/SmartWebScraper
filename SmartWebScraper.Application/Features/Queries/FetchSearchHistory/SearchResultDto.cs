namespace SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
public class SearchResultDto
{
    public string SearchPhrase { get; set; } = default!;
    public string? Positions { get; set; }
    public string? URLs { get; set; }
    public string TargetUrl { get; set; } = default!;
    public DateTime SearchDate { get; set; }
}
