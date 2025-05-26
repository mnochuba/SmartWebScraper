using SmartWebScraper.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SmartWebScraper.Application.Services;
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

        string bingSearchUrl = $"http://www.bing.com/search?q={HttpUtility.UrlEncode(searchPhrase)}&count={_resultCount}";
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");

        var response = await _httpClient.GetStringAsync(bingSearchUrl);

        var myResult = ParseBingResults(response, targetUrl);

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
