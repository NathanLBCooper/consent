using FluentValidation;

namespace Consent.Api.Models
{
    public record WorkspaceCreateRequestModel
    {
        public string? Name { get; init; }
    }

    public class WorkspaceCreateRequestModelValidator : AbstractValidator<WorkspaceCreateRequestModel>
    {
        public WorkspaceCreateRequestModelValidator()
        {
            RuleFor(q => q).NotEmpty();
            RuleFor(q => q.Name).NotNull().NotEmpty();
        }
    }
}
