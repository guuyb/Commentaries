using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commentaries.Api.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("error")]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();


        if (context is not null)
        {
            _logger.LogError(context.Error, context.Error.Message);
        }

        return Problem(
            statusCode: 500,
            title: "Произошла ошибка.");
    }
}
