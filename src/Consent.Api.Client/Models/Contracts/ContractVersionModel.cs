namespace Consent.Api.Client.Models.Contracts;

public record ContractVersionModel(
    int Id, string Name, string Text, ContractVersionStatusModel Status,
    ProvisionModel[] Provisions, ResourceLink Contract
    );
