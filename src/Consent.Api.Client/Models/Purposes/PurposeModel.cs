namespace Consent.Api.Client.Models.Purposes;

public record PurposeModel(
    int Id,
    string Name,
    string Description,
    ResourceLink Workspace
    );
