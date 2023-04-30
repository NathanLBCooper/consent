using Consent.Api.Client.Models.Contracts;
using FluentValidation;

namespace Consent.Api.Contracts;

public class ContractCreateRequestModelValidator : AbstractValidator<ContractCreateRequestModel>
{
    public ContractCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
