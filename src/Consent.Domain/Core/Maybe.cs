using System;

namespace Consent.Domain.Core;

public record Maybe<TValue>
{
    public TValue? Value { get; }
    public bool HasValue { get; }
    public TValue Unwrap() => HasValue
        ? Value!
        : throw new InvalidOperationException($"{nameof(Value)} does not exist");

    private Maybe(TValue? value)
    {
        Value = value;
        HasValue = true;
    }

    private Maybe()
    {
        Value = default;
        HasValue = false;
    }

    public static Maybe<TValue> None => new();
    public static Maybe<TValue> Some(TValue value) => new(value);
}
