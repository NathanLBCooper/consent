using System;
using System.Threading.Tasks;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public static class ResultExtensions
{
    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.UnwrapError());
    }

    public static Result Then(this Result result, Func<Result> next)
    {
        return result.IsFailure ? result : next();
    }

    public static Result<TOut> Then<TOut>(this Result result, Func<Result<TOut>> next)
    {
        return result.IsFailure ? Result<TOut>.Failure(result.UnwrapError()) : next();
    }
}

public static class ResultTExtensions
{
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Unwrap()) : onFailure(result.UnwrapError());
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> next)
    {
        return result.IsFailure ? Result<TOut>.Failure(result.UnwrapError()) : next(result.Unwrap());
    }

    public static Result Then<TIn>(this Result<TIn> result, Func<TIn, Result> next)
    {
        return result.IsFailure ? Result.Failure(result.UnwrapError()) : next(result.Unwrap());
    }
}

public static class ResultExtensions_Async
{
    public static Task<Result> Then(this Result result, Func<Task<Result>> next)
    {
        return result.IsFailure ? Task.FromResult(result) : next();
    }
    public static Task<Result> Then(this Task<Result> task, Func<Task<Result>> next) => task.ContinueWith(t => Then(t.Result, next)).Unwrap();

    public static Task<Result<TOut>> Then<TOut>(this Result result, Func<Task<Result<TOut>>> next)
    {
        return result.IsFailure ? Task.FromResult(Result<TOut>.Failure(result.UnwrapError())) : next();
    }
    public static Task<Result<TOut>> Then<TOut>(this Task<Result> task, Func<Task<Result<TOut>>> next) => task.ContinueWith(t => Then(t.Result, next)).Unwrap();

    public static Task<Result> Then(this Task<Result> task, Func<Result> next) => task.ContinueWith(t => ResultExtensions.Then(t.Result, next));
    public static Task<Result<TOut>> Then<TOut>(this Task<Result> task, Func<Result<TOut>> next) => task.ContinueWith(t => ResultExtensions.Then(t.Result, next));
}

public static class ResultTExtensions_Async
{
    public static Task<Result<TOut>> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> next)
    {
        return result.IsFailure ? Task.FromResult(Result<TOut>.Failure(result.UnwrapError())) : next(result.Unwrap());
    }
    public static Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> task, Func<TIn, Task<Result<TOut>>> next) => task.ContinueWith(t => Then(t.Result, next)).Unwrap();

    public static Task<Result> Then<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> next)
    {
        return result.IsFailure ? Task.FromResult(Result.Failure(result.UnwrapError())) : next(result.Unwrap());
    }
    public static Task<Result> Then<TIn>(this Task<Result<TIn>> task, Func<TIn, Task<Result>> next) => task.ContinueWith(t => Then(t.Result, next)).Unwrap();

    public static Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> task, Func<TIn, Result<TOut>> next) => task.ContinueWith(t => ResultTExtensions.Then(t.Result, next));
    public static Task<Result> Then<TIn>(this Task<Result<TIn>> task, Func<TIn, Result> next) => task.ContinueWith(t => ResultTExtensions.Then(t.Result, next));
}

public static class ResultUtil
{
    public static Func<Result> Succeed(Action action)
    {
        return () => { action(); return Result.Success(); };
    }

    public static Func<TIn, Result> Succeed<TIn>(Action<TIn> func)
    {
        return (@in) => { func(@in); return Result.Success(); };
    }

    public static Func<TIn, Result<TOut>> Succeed<TIn, TOut>(Func<TIn, TOut> func)
    {
        return (@in) => { var @out = func(@in); return Result<TOut>.Success(@out); };
    }

    public static Func<Result<TOut>> Succeed<TOut>(Func<TOut> func)
    {
        return () => { var @out = func(); return Result<TOut>.Success(@out); };
    }

    public static Func<Task<Result>> Succeed(Func<Task> func)
    {
        return async () => { await func(); return Result.Success(); };
    }

    public static Func<Task<Result<TOut>>> Succeed<TOut>(Func<Task<TOut>> func)
    {
        return async () => await func().ContinueWith(@out => Result<TOut>.Success(@out.Result));
    }

    public static Func<TIn, Task<Result<TOut>>> Succeed<TIn, TOut>(Func<TIn, Task<TOut>> func)
    {
        return async (@in) => await func(@in).ContinueWith(@out => Result<TOut>.Success(@out.Result));
    }
}
