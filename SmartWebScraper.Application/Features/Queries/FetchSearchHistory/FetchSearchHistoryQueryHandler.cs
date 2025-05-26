using MediatR;
using SmartWebScraper.Application.Contracts;
using SmartWebScraper.Application.Utilities;
using SmartWebScraper.Domain;
using System.Net;

namespace SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
public class FetchSearchHistoryQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FetchSearchHistoryQuery, OperationResult<List<SearchResultDto>>>
{
    public async Task<OperationResult<List<SearchResultDto>>> Handle(FetchSearchHistoryQuery request, CancellationToken cancellationToken)
    {
        var searchResults = await _unitOfWork.SearchResultRepository.GetAllsync(cancellationToken);
        if (searchResults == null || !searchResults.Any())
        {
            return OperationResult<List<SearchResultDto>>.Failure(HttpStatusCode.NotFound)
                .AddError("Search history is empty");
        }
        var searchResultDtos = searchResults.Select(sr => new SearchResultDto
        {
            SearchPhrase = sr.SearchPhrase,
            Positions = sr.Positions,
            URLs = sr.URLs,
            TargetUrl = sr.TargetUrl,
            SearchDate = sr.SearchDate
        }).ToList();

        return OperationResult<List<SearchResultDto>>.Success(searchResultDtos);
    }

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
}
