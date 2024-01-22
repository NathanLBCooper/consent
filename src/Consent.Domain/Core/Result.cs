using System;
using System.Diagnostics.CodeAnalysis;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Core;

public record Result
{
    public Error? Error { get; }
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool IsSuccess => Error is null;
    [MemberNotNullWhen(returnValue: true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    protected Result(Error? error = null)
    {
        Error = error;
    }

    public void Unwrap()
    {
        if (IsFailure)
        {
            throw new InvalidOperationException(
                $"Called {nameof(Unwrap)} on {nameof(Error)} value: {Error}");
        }
    }

    public virtual Error UnwrapError()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException(
                $"Called {nameof(UnwrapError)} on successful result");
        }

        return Error;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);
}

public record Result<TValue> : Result
{
    private readonly Maybe<TValue> _value;

    protected Result(TValue value)
    {
        _value = Maybe<TValue>.Some(value);
    }

    protected Result(Error error) : base(error)
    {
        _value = Maybe<TValue>.None;
    }

    public new TValue Unwrap()
    {
        base.Unwrap();
        return _value.Value;
    }

    public override Error UnwrapError()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException(
                $"Called {nameof(UnwrapError)} on successful result: {_value}");
        }

        return Error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(Error error) => new(error);
}
