using System;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public record Result
{
    protected Maybe<Error> Error { get; }

    public bool IsSuccess => !Error.HasValue;
    public bool IsFailure => !IsSuccess;

    public void Unwrap()
    {
        if (IsFailure)
        {
            throw new InvalidOperationException(
                $"Called {nameof(Unwrap)} on {nameof(Error)} value: {Error.Value}");
        }
    }

    public virtual Error UnwrapError()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException(
                $"Called {nameof(UnwrapError)} on successful result");
        }

        return Error.Value;
    }

    protected Result()
    {
        Error = Maybe<Error>.None;
    }

    protected Result(Error error)
    {
        Error = Maybe<Error>.Some(error);
    }

    public static Result Success() => new Result();
    public static Result Failure(Error error) => new Result(error);
}

public record Result<TValue> : Result
{
    private Maybe<TValue> Value { get; }

    public new TValue Unwrap()
    {
        if (IsFailure)
        {
            throw new InvalidOperationException(
                $"Called {nameof(Unwrap)} on {nameof(Error)} value: {Error.Value}");
        }

        return Value.Value;
    }

    public override Error UnwrapError()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException(
                $"Called {nameof(UnwrapError)} on successful result: {Value.Value}");
        }

        return Error.Value;
    }

    private Result(Error error) : base(error)
    {
        Value = Maybe<TValue>.None;
    }

    protected Result(TValue value) : base()
    {
        Value = Maybe<TValue>.Some(value);
    }

    public static Result<TValue> Success(TValue value) => new Result<TValue>(value);
    public new static Result<TValue> Failure(Error error) => new Result<TValue>(error);
}
