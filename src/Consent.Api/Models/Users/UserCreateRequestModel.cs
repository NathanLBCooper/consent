using FluentValidation;

namespace Consent.Api.Models.Users;

public record UserCreateRequestModel(
    string? Name
    );

public class UserCreateRequestModelValidator : AbstractValidator<UserCreateRequestModel>
{
    public UserCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
