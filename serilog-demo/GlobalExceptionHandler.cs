using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace serilog_demo;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError
        };

        // if (exception is ValidationException fluentEx)
        // {
        //     problemDetails.Title = "one or more validation errors occurred.";
        //     problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        //     httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //
        //     problemDetails.Extensions.Add("errors", fluentEx.Errors);
        // }


        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            });
    }
}