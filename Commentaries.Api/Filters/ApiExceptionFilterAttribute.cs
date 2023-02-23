using Commentaries.Api.MvcProblemDetails;
using Commentaries.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using ValidationException = Commentaries.Domain.Common.Exceptions.ValidationException;

namespace Commentaries.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly IWebHostEnvironment _env;

    public ApiExceptionFilterAttribute(IWebHostEnvironment env)
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
        };

        _env = env;
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            context.ExceptionHandled = true;
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as ValidationException;
        _ = exception ?? throw new InvalidOperationException("Exception is expected");

        var details = new CustomValidationProblemDetails(
            exception.Errors,
            exception.ErrorMessages.Select(x => x.LocalizedMessage).ToArray())
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = context.Exception as NotFoundException;
        _ = exception ?? throw new InvalidOperationException("Exception is expected");

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.LocalizedMessage,
        };

        context.Result = new NotFoundObjectResult(details);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Произошла ошибка.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        if (_env.IsDevelopment())
        {
            details.Title = $"type: {context.Exception.GetType().FullName} message: {context.Exception.Message}";
            details.Detail = context.Exception.ToString();
        }

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
