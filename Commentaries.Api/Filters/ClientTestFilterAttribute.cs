using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;

namespace Commentaries.Api.Filters;

internal class ClientTestFilterAttribute : Attribute, IActionFilter
{
    private readonly IWebHostEnvironment _env;

    public ClientTestFilterAttribute(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // for client testing purposes
        if (_env.IsDevelopment()
            && context.HttpContext.Request.Headers.ContainsKey("client-test"))
        {
            context.Result = new StatusCodeResult(418); // I'm a teapot
        }
    }
}
