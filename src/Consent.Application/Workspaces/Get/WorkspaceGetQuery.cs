using Consent.Domain.Users;
using Consent.Domain.Workspaces;

namespace Consent.Application.Workspaces.Get;

public record WorkspaceGetQuery(
    WorkspaceId WorkspaceId,
    UserId RequestedBy
    );
