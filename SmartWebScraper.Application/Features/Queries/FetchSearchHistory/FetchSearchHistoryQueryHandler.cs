using MediatR;
using SmartWebScraper.Domain;
using SmartWebScraper.Domain.Contracts;
using SmartWebScraper.Domain.Utilities;
using System.Net;

namespace SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
public class FetchSearchHistoryQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FetchSearchHistoryQuery, OperationResult<List<SearchResultDto>>>
{
    public async Task<OperationResult<List<SearchResultDto>>> Handle(FetchSearchHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchResults = await _unitOfWork.SearchResultRepository.GetAllsync(cancellationToken);

            if (searchResults == null || !searchResults.Any())
            {
                return OperationResult<List<SearchResultDto>>.Failure(HttpStatusCode.NotFound)
                    .AddError("Search history is empty");
            }
            var searchResultDtos = searchResults.Select(sr =>
            {
                // Split positions and URLs into collections
                var positions = sr.Positions.SplitIntoCollection().Select(position => position).ToList();
                var urls = sr.URLs.SplitIntoStringCollection().ToList();
                var rankings = positions.Zip(urls, (position, url) => new { position, url })
                                       .ToDictionary(x => x.position, x => x.url);

                return new SearchResultDto
                {
                    SearchPhrase = sr.SearchPhrase,
                    Rankings = rankings,
                    TargetUrl = sr.TargetUrl,
                    SearchDate = sr.SearchDate
                };
            }).ToList();

            return OperationResult<List<SearchResultDto>>.Success(searchResultDtos);

        }
        catch (Exception ex)
        {
            return OperationResult<List<SearchResultDto>>.Failure(HttpStatusCode.InternalServerError)
                .AddError($"An error occurred while saving the search result: {ex.Message}");
        }
    }

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
}
