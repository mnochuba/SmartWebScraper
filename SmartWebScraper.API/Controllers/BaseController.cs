using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartWebScraper.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }
    protected IMediator Mediator { get; }
}
