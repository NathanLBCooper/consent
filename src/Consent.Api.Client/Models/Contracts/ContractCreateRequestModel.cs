namespace Consent.Api.Client.Models.Contracts;

public record ContractCreateRequestModel(
    string? Name, int WorkspaceId
    );
