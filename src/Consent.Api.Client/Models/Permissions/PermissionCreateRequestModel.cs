namespace Consent.Api.Client.Models.Permissions;

public record PermissionCreateRequestModel(
    string? Name,
    string? Description,
    int WorkspaceId
    );
