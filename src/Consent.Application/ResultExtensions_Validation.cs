using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using FluentValidation.Results;

namespace Consent.Application;

internal static class ResultExtensions_Validation
{
    public static Result ToResult(this ValidationResult validationResult)
    {
        return validationResult.IsValid ? Result.Success() : Result.Failure(new ValidationError(validationResult.ToString()));
    }
}
