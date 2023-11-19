namespace Consent.Api.Client.Models.Permissions;

public record PermissionModel(
    int Id,
    string Name,
    string Description,
    ResourceLink Workspace
    );
