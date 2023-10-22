using Consent.Api.Client.Models.Contracts;
using FluentValidation;

namespace Consent.Api.Contracts;

public class ProvisionCreateRequestModelValidator : AbstractValidator<ProvisionCreateRequestModel>
{
    public ProvisionCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Text).NotNull().NotEmpty();
        _ = RuleFor(q => q.PermissionIds).NotEmpty();
    }
}
