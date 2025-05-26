using MediatR;
using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.Application.Features.Commands.SearchForKeyword;
public class SearchForKeywordCommand : IRequest<OperationResult<Dictionary<int, string>>>
{
    public string SearchPhrase { get; set; } = default!;
    public string TargetUrl { get; set; } = "infotrack.co.uk";
}
