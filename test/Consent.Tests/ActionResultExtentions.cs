using Microsoft.AspNetCore.Mvc;

namespace Consent.Tests;

internal static class ActionResultExtentions
{
    public static TValue? GetValue<TValue>(this ActionResult<TValue> result) where TValue : class
    {
        return (result.Result as OkObjectResult)?.Value as TValue;
    }
}
