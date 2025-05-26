using MediatR;
using SmartWebScraper.Application.Utilities;

namespace SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
public class FetchSearchHistoryQuery : IRequest<OperationResult<List<SearchResultDto>>>
{
}