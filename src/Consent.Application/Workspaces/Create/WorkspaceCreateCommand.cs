using Consent.Domain.Users;

namespace Consent.Application.Workspaces.Create;

public record WorkspaceCreateCommand(
    string? Name,
    UserId RequestedBy
    );
