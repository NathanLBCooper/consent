using System;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public static class ResultExtensions
{
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Unwrap()) : onFailure(result.UnwrapError());
    }

    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.UnwrapError());
    }

    public static Result Then(this Result result, Func<Result> next)
    {
        return result.IsFailure ? result : next();
    }
    public static Result Then(this Result result, Action next) => Then(result, () => Succeed(next));

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> next)
    {
        return result.IsFailure ? Result<TOut>.Failure(result.UnwrapError()) : next(result.Unwrap());
    }
    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> next) => Then(result, @in => Succeed(() => next(@in)));

    public static Result Then<TIn>(this Result<TIn> result, Func<TIn, Result> next)
    {
        return result.IsFailure ? Result.Failure(result.UnwrapError()) : next(result.Unwrap());
    }
    public static Result Then<TIn>(this Result<TIn> result, Action<TIn> next) => Then(result, @in => Succeed(() => next(@in)));

    public static Result<TOut> Then<TOut>(this Result result, Func<Result<TOut>> next)
    {
        return result.IsFailure ? Result<TOut>.Failure(result.UnwrapError()) : next();
    }
    public static Result<TOut> Then<TOut>(this Result result, Func<TOut> next) => Then(result, () => Succeed(() => next()));

    private static Result Succeed(Action action)
    {
        action();
        return Result.Success();
    }

    private static Result<TOut> Succeed<TOut>(Func<TOut> func)
    {
        return Result<TOut>.Success(func());
    }
}
