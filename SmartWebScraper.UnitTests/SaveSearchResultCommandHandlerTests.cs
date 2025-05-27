using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SmartWebScraper.Application.Features.Commands.SaveSearchResult;
using SmartWebScraper.Domain;
using SmartWebScraper.Domain.Contracts;
using SmartWebScraper.Domain.Utilities;
using System.Net;
using Xunit;

namespace SmartWebScraper.UnitTests;

public class SaveSearchResultCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<SaveSearchResultCommand>> _validatorMock;
    private readonly SaveSearchResultCommandHandler _handler;

    public SaveSearchResultCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<SaveSearchResultCommand>>();
        _handler = new SaveSearchResultCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidCommandAndSaveSucceeds()
    {
        // Arrange
        var command = CreateValidCommand();
        SetupValidatorToSucceed(command);
        SetupRepositoryAddAsyncToSucceed();
        SetupSaveChangesAsyncToReturn(OperationResult.Success());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().NotBe(null);
        result.Errors.Should().BeEmpty();
        result.Code.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new SaveSearchResultCommand();
        const string errorMsg = "Search phrase is required.";
        SetupValidatorToFail(command, "SearchPhrase", errorMsg);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.BadRequest);
        result.Errors.Should().Contain(errorMsg);
        result.Data.Should().Be(default);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSaveChangesFails()
    {
        // Arrange
        var command = CreateValidCommand();
        SetupValidatorToSucceed(command);
        SetupRepositoryAddAsyncToSucceed();
        SetupSaveChangesAsyncToReturn(OperationResult.Failure(HttpStatusCode.InternalServerError));

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.InternalServerError);
        result.Data.Should().Be(default);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var command = CreateValidCommand();
        SetupValidatorToSucceed(command);
        SetupRepositoryAddAsyncToThrow(new Exception("Unexpected error"));

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Code.Should().Be(HttpStatusCode.InternalServerError);
        result.Errors.Should().Contain(e => e.Contains("An error occurred while saving the search result"));
        result.Data.Should().Be(default);
    }

    private SaveSearchResultCommand CreateValidCommand() =>
    new()
    {
        SearchPhrase = "test",
        TargetUrl = "www.example.com",
        Rankings = new() { { 1, "https://example.com" } }
    };

    private void SetupValidatorToSucceed(SaveSearchResultCommand command) =>
        _validatorMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

    private void SetupValidatorToFail(SaveSearchResultCommand command, string property, string error) =>
        _validatorMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new(property, error) }));

    private void SetupRepositoryAddAsyncToSucceed() =>
        _unitOfWorkMock.Setup(u => u.SearchResultRepository.AddAsync(It.IsAny<SearchResult>(), default))
            .Returns(Task.CompletedTask);

    private void SetupRepositoryAddAsyncToThrow(Exception ex) =>
        _unitOfWorkMock.Setup(u => u.SearchResultRepository.AddAsync(It.IsAny<SearchResult>(), default))
            .ThrowsAsync(ex);

    private void SetupSaveChangesAsyncToReturn(OperationResult result) =>
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(result);

}