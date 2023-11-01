using FluentValidation;

namespace Consent.Application.Workspaces;

public class WorkspaceCreateCommandValidator : AbstractValidator<WorkspaceCreateCommand>
{
    public WorkspaceCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotEmpty();
    }
}
