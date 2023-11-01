using Consent.Domain.Users;

namespace Consent.Application.Workspaces;

public record WorkspaceCreateCommand(
    string? Name,
    UserId RequestedBy
    );
