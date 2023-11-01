using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Application.Workspaces;

public record WorkspaceGetQuery(
    WorkspaceId WorkspaceId,
    UserId RequestedBy
    );
