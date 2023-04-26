using FluentValidation;

namespace Consent.Api.Models.Workspaces;

public record WorkspaceCreateRequestModel(
    string? Name
    );

public class WorkspaceCreateRequestModelValidator : AbstractValidator<WorkspaceCreateRequestModel>
{
    public WorkspaceCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
