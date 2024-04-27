using System;

namespace Consent.Domain.Core;

public static class MaybeExtensions
{
    public static TOut Match<TIn, TOut>(this Maybe<TIn> result, Func<TIn, TOut> onSome, Func<TOut> onNone)
    {
        return result.HasValue ? onSome(result.Unwrap()) : onNone();
    }
}
