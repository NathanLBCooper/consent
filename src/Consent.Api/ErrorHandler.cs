using System;
using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Api;

public class ErrorHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not Error error)
        {
            return false;
        }

        await ToActionResult(error)
            .ExecuteResultAsync(new ActionContext
            {
                HttpContext = httpContext
            });

        return true;
    }

    private static ActionResult ToActionResult(Error error)
    {
        return error switch
        {
            NotFoundError => error.Detail.HasValue ? new NotFoundObjectResult(error.Message) : new NotFoundResult(),
            UnauthorizedError => error.Detail.HasValue ? new UnauthorizedObjectResult(error.Message) : new UnauthorizedResult(),
            ValidationError => error.Detail.HasValue ? new UnprocessableEntityObjectResult(error.Message) : new UnprocessableEntityResult(),
            _ => error.Detail.HasValue ? new BadRequestObjectResult(error.Message) : new BadRequestResult()
        };
    }
}
