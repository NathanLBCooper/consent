using FluentValidation;

namespace Consent.Application.Contracts.Create;

internal class ContractCreateCommandValidator : AbstractValidator<ContractCreateCommand>
{
    public ContractCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
    }
}
