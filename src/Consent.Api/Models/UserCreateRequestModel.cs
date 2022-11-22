using FluentValidation;

namespace Consent.Api.Models
{
    public record UserCreateRequestModel
    {
        public string? Name { get; init; }
    }

    public class UserCreateRequestModelValidator : AbstractValidator<UserCreateRequestModel>
    {
        public UserCreateRequestModelValidator()
        {
            RuleFor(q => q).NotEmpty();
            RuleFor(q => q.Name).NotNull().NotEmpty();
        }
    }
}
