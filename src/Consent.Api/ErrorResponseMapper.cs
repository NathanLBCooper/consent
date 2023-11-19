using Consent.Domain.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Api;

internal static class ErrorResponseMapper
{
    public static ActionResult<TModel> ToErrorResponse<TModel>(this Error error, ControllerBase controller)
    {
        return error switch
        {
            NotFoundError => error.Message is not null ? controller.NotFound(error.Message) : controller.NotFound(),
            UnauthorizedError => error.Message is not null ? controller.Unauthorized(error.Message) : controller.Unauthorized(),
            ValidationError => error.Message is not null ? controller.UnprocessableEntity(error.Message) : controller.UnprocessableEntity(),
            _ => error.Message is not null ? controller.BadRequest(error.Message) : controller.BadRequest()
        };
    }
}
