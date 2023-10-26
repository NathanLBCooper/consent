using System;
using System.Linq;
using Consent.Api.Client.Models;
using Consent.Api.Client.Models.Contracts;
using Consent.Domain.Contracts;
using Consent.Domain.Core;

namespace Consent.Api.Contracts;

internal static class ContractModelMapper
{
    public static ContractModel ToModel(this Contract contract, ConsentLinkGenerator linkGenerator)
    {
        var workspace = new ResourceLink(contract.WorkspaceId.Value, linkGenerator.GetWorkspace(contract.WorkspaceId));
        var contractId = Guard.NotNull(contract.Id);
        var versionLinks = contract.Versions.Select(
            v =>
            {
                var versionId = Guard.NotNull(v.Id);
                return new ResourceLink(versionId.Value, linkGenerator.GetContractVersion(contractId, versionId));
            }).ToArray();

        return new ContractModel(
            Id: contractId.Value,
            Name: contract.Name,
            Workspace: workspace,
            Versions: versionLinks
            );
    }

    public static ContractVersionModel ToModel(
        this ContractVersion version, Contract contract, ConsentLinkGenerator linkGenerator)
    {
        var versionId = Guard.NotNull(version.Id);
        var contractId = Guard.NotNull(contract.Id);
        var contractLink = new ResourceLink(contractId.Value, linkGenerator.GetContract(contractId));
        var provisions = version.Provisions.Select(p => p.ToModel(version, contract, linkGenerator)).ToArray();

        return new ContractVersionModel(versionId.Value, version.Name, version.Text, version.Status.ToModel(), provisions, contractLink);
    }

    public static ProvisionModel ToModel(this Provision provision, ContractVersion version, Contract contract,
        ConsentLinkGenerator linkGenerator)
    {
        var provisionId = Guard.NotNull(provision.Id);
        var versionId = Guard.NotNull(version.Id);
        var contractId = Guard.NotNull(contract.Id);
        var permissionLinks = provision.PermissionIds.Select(
            pid => new ResourceLink(pid.Value, linkGenerator.GetPermission(pid))
            ).ToArray();
        var versionLink = new ResourceLink(versionId.Value, linkGenerator.GetContractVersion(contractId, versionId));

        return new ProvisionModel(provisionId.Value, provision.Text, permissionLinks, versionLink);
    }

    private static ContractVersionStatusModel ToModel(this ContractVersionStatus status)
    {
        return status switch
        {
            ContractVersionStatus.Draft => ContractVersionStatusModel.Draft,
            ContractVersionStatus.Active => ContractVersionStatusModel.Active,
            ContractVersionStatus.Legacy => ContractVersionStatusModel.Legacy,
            ContractVersionStatus.Deprecated => ContractVersionStatusModel.Deprecated,
            ContractVersionStatus.Obsolete => ContractVersionStatusModel.Obsolete,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
}
