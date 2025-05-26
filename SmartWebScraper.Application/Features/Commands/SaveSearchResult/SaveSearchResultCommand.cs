using MediatR;
using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.Application.Features.Commands.SaveSearchResult;
public class SaveSearchResultCommand() : IRequest<OperationResult<int>>
{
    public string SearchPhrase { get; set; } = default!;
    public string TargetUrl { get; set; } = "infotrack.co.uk";
    public Dictionary<int, string> Rankings { get; set; } = [];
}
