using System;

namespace Consent.Domain.Core.Errors;

public class Error : Exception
{
    public Maybe<string> Detail { get; }

    public Error()
    {
        Detail = Maybe<string>.None;
    }

    public Error(string detail) : base(detail)
    {
        Detail = detail;
    }

    public Error(string detail, Exception innerException) : base(detail, innerException)
    {
        Detail = detail;
    }
}
