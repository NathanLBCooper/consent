using System;
using System.Linq;
using Consent.Api.Client.Models;
using Consent.Api.Client.Models.Contracts;
using Consent.Domain;
using Consent.Domain.Contracts;

namespace Consent.Api.Contracts;

internal static class ContractModelMapper
{
    public static ContractModel ToModel(this Contract entity, ConsentLinkGenerator linkGenerator)
    {
        var workspace = new ResourceLink(entity.WorkspaceId.Value, linkGenerator.GetWorkspace(entity.WorkspaceId));
        var contractId = Guard.NotNull(entity.Id);
        var versionLinks = entity.Versions.Select(
            v =>
            {
                var id = Guard.NotNull(v.Id);
                return new ResourceLink(id.Value, linkGenerator.GetContractVersion(contractId, id));
            }).ToArray();

        return new ContractModel(
            Id: entity.Id!.Value.Value,
            Name: entity.Name,
            Workspace: workspace,
            Versions: versionLinks
            );
    }

    public static ContractVersionModel ToModel(
        this ContractVersion entity, Contract contract, ConsentLinkGenerator linkGenerator)
    {
        var versionId = Guard.NotNull(entity.Id);
        var contractId = Guard.NotNull(contract.Id);
        var contractLink = new ResourceLink(contractId.Value, linkGenerator.GetContract(contractId));

        return new ContractVersionModel(versionId.Value, entity.Name, entity.Text, entity.Status.ToModel(), contractLink);
    }

    private static ContractVersionStatusModel ToModel(this ContractVersionStatus value)
    {
        return value switch
        {
            ContractVersionStatus.Draft => ContractVersionStatusModel.Draft,
            ContractVersionStatus.Active => ContractVersionStatusModel.Active,
            ContractVersionStatus.Legacy => ContractVersionStatusModel.Legacy,
            ContractVersionStatus.Deprecated => ContractVersionStatusModel.Deprecated,
            ContractVersionStatus.Obsolete => ContractVersionStatusModel.Obsolete,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}
