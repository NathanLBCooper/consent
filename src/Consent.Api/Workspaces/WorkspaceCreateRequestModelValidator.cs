using Consent.Api.Client.Models.Workspaces;
using FluentValidation;

namespace Consent.Api.Workspaces;

public class WorkspaceCreateRequestModelValidator : AbstractValidator<WorkspaceCreateRequestModel>
{
    public WorkspaceCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
