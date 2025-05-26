using FluentValidation;
using MediatR;
using SmartWebScraper.Infrastructure.Services;
using SmartWebScraper.Domain.Utilities;
using System.Net;

namespace SmartWebScraper.Application.Features.Commands.SearchForKeyword;
public class SearchForKeywordCommandHandler(IHttpClientFactory httpClientFactory, IValidator<SearchForKeywordCommand> validator)
                                            : IRequestHandler<SearchForKeywordCommand, OperationResult<Dictionary<int, string>>>
{
    public async Task<OperationResult<Dictionary<int, string>>> Handle(SearchForKeywordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return OperationResult<Dictionary<int, string>>.Failure(HttpStatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
        }
        try
        {
            var searchEngineScraper = new BingScraper(_httpClientFactory);
            var result = await searchEngineScraper.GetSearchResultPositionsAsync(request.SearchPhrase, request.TargetUrl, cancellationToken);

            return OperationResult<Dictionary<int, string>>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<Dictionary<int, string>>.Failure(HttpStatusCode.InternalServerError)
                .AddError($"An error occurred while processing the search: {ex.Message}");
        }
    }

    //private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<SearchForKeywordCommand> _validator = validator;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
}
