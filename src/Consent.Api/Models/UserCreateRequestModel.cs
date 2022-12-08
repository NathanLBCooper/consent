using FluentValidation;

namespace Consent.Api.Models;

public record UserCreateRequestModel
{
    public string? Name { get; init; }
}

public class UserCreateRequestModelValidator : AbstractValidator<UserCreateRequestModel>
{
    public UserCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
