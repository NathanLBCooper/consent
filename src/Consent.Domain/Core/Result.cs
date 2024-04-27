using System;
using System.Diagnostics.CodeAnalysis;
using Consent.Domain.Core.Errors;

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
    private readonly Maybe<TValue> _maybe;

    public TValue? Value => _maybe.Value;
    public TValue Unwrap() => _maybe.HasValue
        ? _maybe.Unwrap()
        : throw new InvalidOperationException($"Failure Result has no {nameof(Value)}");

    private Result(TValue value)
    {
        _maybe = Maybe<TValue>.Some(value);
    }

    private Result(Error error) : base(error)
    {
        _maybe = Maybe<TValue>.None;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static new Result<TValue> Failure(Error error) => new(error);
}
