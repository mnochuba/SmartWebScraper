using MediatR;
using SmartWebScraper.Application.Utilities;

namespace SmartWebScraper.Application.Features.Commands.SearchForKeyword;
public class SearchForKeywordCommand : IRequest<OperationResult<List<int>>>
{
    public string SearchPhrase { get; set; } = default!;
    public string TargetUrl { get; set; } = "infotrack.co.uk";
}
