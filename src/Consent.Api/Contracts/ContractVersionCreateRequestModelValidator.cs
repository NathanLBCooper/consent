using Consent.Api.Client.Models.Contracts;
using FluentValidation;

namespace Consent.Api.Contracts;

public class ContractVersionCreateRequestModelValidator : AbstractValidator<ContractVersionCreateRequestModel>
{
    public ContractVersionCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
        _ = RuleFor(q => q.Text).NotNull().NotEmpty();
    }
}
