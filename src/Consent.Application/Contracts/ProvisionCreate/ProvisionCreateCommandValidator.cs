using FluentValidation;

namespace Consent.Application.Contracts.ProvisionCreate;

internal class ProvisionCreateCommandValidator : AbstractValidator<ProvisionCreateCommand>
{
    public ProvisionCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Text).NotEmpty();
        _ = RuleFor(q => q.PermissionIds).NotEmpty();
    }
}
