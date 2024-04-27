using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Consent.Domain.Core;

public static class Guard
{
    [return: NotNull]
    public static TValue NotNull<TValue>(TValue? value, [CallerArgumentExpression(nameof(value))] string? valueName = null) where TValue : class
    {
        return value ?? throw new System.ArgumentNullException(valueName);
    }

    [return: NotNull]
    public static TValue NotNull<TValue>(TValue? value, [CallerArgumentExpression(nameof(value))] string? valueName = null) where TValue : struct
    {
        return value ?? throw new System.ArgumentNullException(valueName);
    }
}
