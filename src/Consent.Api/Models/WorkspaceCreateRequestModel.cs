﻿using FluentValidation;

namespace Consent.Api.Models;

public record WorkspaceCreateRequestModel
{
    public string? Name { get; init; }
}

public class WorkspaceCreateRequestModelValidator : AbstractValidator<WorkspaceCreateRequestModel>
{
    public WorkspaceCreateRequestModelValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        _ = RuleFor(q => q.Name).NotNull().NotEmpty();
    }
}
