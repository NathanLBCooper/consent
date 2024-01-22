using System;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public static class ResultExtensions
{
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Unwrap()) : onFailure(result.Error);
    }

    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static Result Bind(this Result result, Func<Result> next)
    {
        return result.IsFailure ? result : next();
    }

    public static Result<T2> Bind<T1, T2>(this Result<T1> result, Func<Result<T2>> next)
    {
        return result.IsFailure ? Result<T2>.Failure(result.Error) : next();
    }

    public static Result<T> Bind<T>(this Result result, Func<Result<T>> next)
    {
        return result.IsFailure ? Result<T>.Failure(result.Error) : next();
    }

    public static Result Bind<T>(this Result<T> result, Func<Result> next)
    {
        return result.IsFailure ? Result.Failure(result.Error) : next();
    }
}
