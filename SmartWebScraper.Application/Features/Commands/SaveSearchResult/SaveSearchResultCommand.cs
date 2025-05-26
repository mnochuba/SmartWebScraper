using MediatR;
using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.Application.Features.Commands.SaveSearchResult;
public class SaveSearchResultCommand() : IRequest<OperationResult<int>>
{
    public string SearchPhrase { get; set; } = default!;
    public List<int> Positions { get; set; } = [];
    public List<string> URLs { get; set; } = [];
    public string TargetUrl { get; set; } = "infotrack.co.uk";
}
