using System;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public static class ResultExtensions
{
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Unwrap()) : onFailure(result.Error);
    }
}
