using System;

namespace Consent.Domain.Core;

public record Maybe<TValue>
{
    private readonly TValue? _value;
    public bool HasValue { get; }
    public TValue Value => HasValue
        ? _value!
        : throw new InvalidOperationException($"{nameof(Value)} does not exist");
    public TValue? Unwrap() => _value;

    private Maybe(TValue? value)
    {
        _value = value;
        HasValue = true;
    }

    private Maybe()
    {
        _value = default;
        HasValue = false;
    }

    public static Maybe<TValue> None => new();
    public static Maybe<TValue> Some(TValue value) => new(value);

    public static implicit operator Maybe<TValue>(TValue value)
    {
        return value is null ? None : Some(value);
    }
}
