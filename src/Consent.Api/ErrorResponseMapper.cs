using Consent.Application;
using Consent.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Api;

internal static class ErrorResponseMapper
{
    public static ActionResult<TModel> ToErrorResponse<TModel>(this Error error, ControllerBase controller)
    {
        return error switch
        {
            ValidationError => controller.UnprocessableEntity(error.ToString()),
            _ => controller.BadRequest(error.ToString()),
        };
    }
}
