using FluentValidation;

namespace Consent.Application.Users;

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
    }
}
