using FluentAssertions;
using Moq;
using SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
using SmartWebScraper.Domain;
using SmartWebScraper.Domain.Contracts;
using System.Net;
using Xunit;

namespace SmartWebScraper.UnitTests;

public class FetchSearchHistoryQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly FetchSearchHistoryQueryHandler _handler;

    public FetchSearchHistoryQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new FetchSearchHistoryQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSearchHistoryExists()
    {
        // Arrange
        var searchResults = new List<SearchResult>
        {
            new("test search 1", [1], ["https://example1.com"], "www.example1.com"),
            new("test search 2", [1], ["https://example2.com"], "www.example2.com")
        };

        _unitOfWorkMock.Setup(u => u.SearchResultRepository.GetAllsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchResults);

        // Act
        var result = await _handler.Handle(new FetchSearchHistoryQuery(), default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Code.Should().Be(HttpStatusCode.OK);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSearchHistoryIsEmpty()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.SearchResultRepository.GetAllsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SearchResult>());

        // Act
        var result = await _handler.Handle(new FetchSearchHistoryQuery(), default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.NotFound);
        result.Data.Should().BeNull();
        result.Errors.Should().Contain("Search history is empty");
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSearchHistoryIsNull()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.SearchResultRepository.GetAllsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<SearchResult>?)null!);

        // Act
        var result = await _handler.Handle(new FetchSearchHistoryQuery(), default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.NotFound);
        result.Data.Should().BeNull();
        result.Errors.Should().Contain("Search history is empty");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.SearchResultRepository.GetAllsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _handler.Handle(new FetchSearchHistoryQuery(), default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.InternalServerError);
        result.Data.Should().BeNull();
        result.Errors.Should().Contain(e => e.Contains("An unexpected error occurred") || e.Contains("Database failure"));
    }
}
