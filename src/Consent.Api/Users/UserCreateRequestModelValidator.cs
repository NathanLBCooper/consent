using Consent.Api.Client.Models.Users;
using FluentValidation;

namespace Consent.Api.Users;

public class UserCreateRequestModelValidator : AbstractValidator<UserCreateRequestModel>
{
    public UserCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
