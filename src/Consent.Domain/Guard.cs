using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Consent.Domain;

public static class Guard
{
    [return: NotNull]
    public static TValue NotNull<TValue>(TValue? value, [CallerArgumentExpression("value")] string? valueName = null)
    {
        if (value == null)
        {
            throw new System.ArgumentNullException(valueName);
        }

        return value;
    }
}
