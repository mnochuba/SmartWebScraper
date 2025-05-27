using System.Text.RegularExpressions;
using System.Web;

namespace SmartWebScraper.Infrastructure.Services;
public class BingScraper //: ISearchEngineScraper
{
    private readonly HttpClient _httpClient;
    private readonly int _resultCount = 100;
    public BingScraper(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://www.bing.com/");
    }

    public async Task<Dictionary<int, string>> GetSearchResultPositionsAsync(string searchPhrase, string targetUrl, CancellationToken cancellationToken)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");

        Dictionary<int, string> tempResult = [];
        Dictionary<int, string> myResult = [];

        for (int i = 1; i < _resultCount; i += 10)
        {
            string bingSearchUrl = $"https://www.bing.com/search?q={HttpUtility.UrlEncode(searchPhrase)}&first={i}";

            var request = new HttpRequestMessage(HttpMethod.Get, bingSearchUrl);

            var response = await _httpClient.SendAsync(request);
            string html = await response.Content.ReadAsStringAsync();

            tempResult = ParseBingResults(html, targetUrl);

            foreach (var kvp in tempResult)
            {
                myResult[kvp.Key + i - 1] = kvp.Value;
            }

            Task.Delay(500, cancellationToken).Wait(); // Delay to avoid hitting the server too fast
        }

        return myResult;
    }

    static Dictionary<int, string> ParseBingResults(string html, string targetUrl)
    {
        var resultDictionary = new Dictionary<int, string>();
        string[] resultBlocks = html.Split(["<li class=\"b_algo\""], StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < resultBlocks.Length; i++)
        {
            string resultHtml = "<li class=\"b_algo\"" + resultBlocks[i];
            var urlMatch = Regex.Match(resultHtml, @"<a\s+[^>]*href\s*=\s*[""']([^""']*?)[""'][^>]*>", RegexOptions.IgnoreCase);

            if (urlMatch.Success)
            {
                string url = urlMatch.Groups[1].Value;
                if (url.Contains(targetUrl, StringComparison.OrdinalIgnoreCase))
                {
                    resultDictionary[i] = url;
                }
            }
        }

        return resultDictionary;
    }
}
