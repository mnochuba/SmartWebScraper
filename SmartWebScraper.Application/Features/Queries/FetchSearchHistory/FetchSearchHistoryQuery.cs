using MediatR;
using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.Application.Features.Queries.FetchSearchHistory;
public class FetchSearchHistoryQuery : IRequest<OperationResult<List<SearchResultDto>>>
{
}