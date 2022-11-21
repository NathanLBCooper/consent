using FluentValidation;

namespace Consent.Api.Models
{
    public record OrganizationCreateRequestModel
    {
        public string? Name { get; init; }
    }

    public class OrganizationCreateRequestModelValidator : AbstractValidator<OrganizationCreateRequestModel>
    {
        public OrganizationCreateRequestModelValidator()
        {
            RuleFor(q => q).NotEmpty();
            RuleFor(q => q.Name).NotNull().NotEmpty();
        }
    }
}
