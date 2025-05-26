using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartWebScraper.Application.Features.Commands.SaveSearchResult;
using SmartWebScraper.Application.Features.Commands.SearchForKeyword;
using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.API.Controllers;

public class SearchController : BaseController
{
    public SearchController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Route("keyword-search")]
    public async Task<IActionResult> SearchForKeyWord(SearchForKeywordCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    [Route("save-results")]
    public async Task<IActionResult> SaveSearchResults(SaveSearchResultCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
}
