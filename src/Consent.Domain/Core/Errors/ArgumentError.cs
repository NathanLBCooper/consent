namespace Consent.Domain.Core.Errors;

public record ArgumentError : Error
{
    public string ParamName { get; }

    public ArgumentError(string paramName, string? message = null) : base(message)
    {
        ParamName = paramName;
    }
}

