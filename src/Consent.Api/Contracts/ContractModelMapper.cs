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
        var versions = entity.Versions.Select(
            v =>
            {
                var id = Guard.NotNull(v.Id);
                return new ResourceLink(id.Value, linkGenerator.GetContractVersion(contractId, id));
            }).ToArray();

        return new ContractModel(
            Id: entity.Id!.Value.Value,
            Name: entity.Name,
            Workspace: workspace,
            Versions: versions
            );
    }
}
