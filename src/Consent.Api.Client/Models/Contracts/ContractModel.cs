namespace Consent.Api.Client.Models.Contracts;

public record ContractModel(
    int Id, string Name, ResourceLink Workspace, ResourceLink[] Versions
    );
