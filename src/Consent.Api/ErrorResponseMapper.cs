using Consent.Domain.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Api;

internal static class ErrorResponseMapper
{
    public static ActionResult<TModel> ToErrorResponse<TModel>(this Error error, ControllerBase controller)
    {
        return error switch
        {
            ValidationError => error.Message != null ? controller.UnprocessableEntity(error.Message) : controller.UnprocessableEntity(),
            UnauthorizedError => error.Message != null ? controller.Unauthorized(error.Message) : controller.Unauthorized(),
            _ => error.Message != null ? controller.BadRequest(error.Message) : controller.BadRequest()
        };
    }
}
