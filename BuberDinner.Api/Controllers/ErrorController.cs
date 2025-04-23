using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error([FromQuery] string? code)
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem();
    }
}