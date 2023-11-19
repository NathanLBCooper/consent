using FluentValidation;

namespace Consent.Application.Contracts.VersionCreate;

internal class ContractVersionCreateCommandValidator : AbstractValidator<ContractVersionCreateCommand>
{
    public ContractVersionCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
        _ = RuleFor(q => q.Text).NotEmpty();
    }
}
