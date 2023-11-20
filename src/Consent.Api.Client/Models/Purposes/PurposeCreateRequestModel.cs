namespace Consent.Api.Client.Models.Purposes;

public record PurposeCreateRequestModel(
    string? Name,
    string? Description,
    int WorkspaceId
    );
