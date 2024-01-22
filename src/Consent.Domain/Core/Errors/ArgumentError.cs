namespace Consent.Domain.Core.Errors;

public record ArgumentError : Error
{
    public string? ParamName { get; }

    public ArgumentError(string? message = null, string? paramName = null) : base(message)
    {
        ParamName = paramName;
    }
}
