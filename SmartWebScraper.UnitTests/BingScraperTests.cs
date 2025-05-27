using Moq;
using Moq.Protected;
using SmartWebScraper.Infrastructure.Services;
using System.Net;
using Xunit;

namespace SmartWebScraper.UnitTests;

public class BingScraperTests
{
    private static IHttpClientFactory CreateHttpClientFactoryWithResponse(string responseContent)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent),
            });

        var client = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
        return factoryMock.Object;
    }

    [Theory]
    [InlineData("another search", "target.com", "<li class=\"b_algo\"><a href=\"https://target.co.uk/page2\">Result 2</a></li>", 0)]
    [InlineData("no match", "notfound.com", "<li class=\"b_algo\"><a href=\"https://other.com/page\">Other</a></li>", 0)]
    public async Task GetSearchResultPositionsAsync_ReturnsExpectedResults(string searchPhrase, string targetUrl, string html, int expectedCount)
    {
        // Arrange
        var factory = CreateHttpClientFactoryWithResponse(html);
        var scraper = new BingScraper(factory);

        // Act
        var result = await scraper.GetSearchResultPositionsAsync(searchPhrase, targetUrl, CancellationToken.None);

        // Assert
        Assert.Equal(expectedCount, result.Count);
        foreach (var url in result.Values)
        {
            Assert.Contains(targetUrl, url, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task GetSearchResultPositionsAsync_HandlesMultipleResults()
    {
        // Arrange
        string html = @"
            <li class=""b_algo""><a href=""https://site1.com"">Site 1</a></li>
            <li class=""b_algo""><a href=""https://target.com/page"">Target</a></li>
            <li class=""b_algo""><a href=""https://target.com/other"">Target 2</a></li>
        ";
        var factory = CreateHttpClientFactoryWithResponse(html);
        var scraper = new BingScraper(factory);

        // Act
        var result = await scraper.GetSearchResultPositionsAsync("search", "target.com", CancellationToken.None, 10);

        // Assert
        Assert.Equal(2, result.Count);
        foreach (var url in result.Values)
        {
            Assert.Contains("target.com", url, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task GetSearchResultPositionsAsync_ReturnsEmpty_WhenNoResults()
    {
        // Arrange
        string html = "<li class=\"b_algo\"><a href=\"https://other.com\">Other</a></li>";
        var factory = CreateHttpClientFactoryWithResponse(html);
        var scraper = new BingScraper(factory);

        // Act
        var result = await scraper.GetSearchResultPositionsAsync("search", "notfound.com", CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
