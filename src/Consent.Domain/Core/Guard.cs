using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Consent.Domain.Core;

public static class Guard
{
    [return: NotNull]
    public static TValue NotNull<TValue>(TValue? value, [CallerArgumentExpression("value")] string? valueName = null) where TValue : class
    {
        if (value == null)
        {
            throw new System.ArgumentNullException(valueName);
        }

        return value;
    }

    [return: NotNull]
    public static TValue NotNull<TValue>(TValue? value, [CallerArgumentExpression("value")] string? valueName = null) where TValue : struct
    {
        if (value == null)
        {
            throw new System.ArgumentNullException(valueName);
        }

        return value.Value;
    }
}
