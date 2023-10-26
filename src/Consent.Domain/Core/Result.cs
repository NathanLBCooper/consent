using System;
using System.Diagnostics.CodeAnalysis;

namespace Consent.Domain.Core;

public record Result
{
    public Error? Error { get; }
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool IsSuccess => Error is null;

    protected Result(Error? error = null)
    {
        Error = error;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);
}

public record Result<TValue> : Result
{
    private readonly Maybe<TValue> _value;
    public TValue Value => _value.HasValue
        ? _value.Value
        : throw new InvalidOperationException($"Failure Result has no {nameof(Value)}");

    protected Result(TValue value)
    {
        _value = Maybe<TValue>.Some(value);
    }

    protected Result(Error error) : base(error)
    {
        _value = Maybe<TValue>.None;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(Error error) => new(error);
}
