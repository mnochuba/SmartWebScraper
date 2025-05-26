using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartWebScraper.Application.Features.Commands.SearchForKeyword;

namespace SmartWebScraper.API.Controllers;

public class SearchController : BaseController
{
    protected SearchController(IMediator mediator) : base(mediator)
    {
    }


    [HttpPost]
    public async Task<IActionResult> SearchForKeyWord(SearchForKeywordCommand request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
}
