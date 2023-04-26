using FluentValidation;

namespace Consent.Api.Models.Contracts;

public record ContractCreateRequestModel(
    string? Name, int WorkspaceId
    );

public class ContractCreateRequestModelValidator : AbstractValidator<ContractCreateRequestModel>
{
    public ContractCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
