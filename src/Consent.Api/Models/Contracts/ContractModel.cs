using System.Linq;
using Consent.Domain;
using Consent.Domain.Contracts;

namespace Consent.Api.Models.Contracts;

public record ContractModel(
    int Id, string Name, ResourceLink Workspace, ResourceLink[] Versions
    );

internal static class ContractModelMapper
{
    public static ContractModel ToModel(this Contract entity, ConsentLinkGenerator linkGenerator)
    {
        var workspace = new ResourceLink(entity.WorkspaceId.Value, linkGenerator.GetWorkspace(entity.WorkspaceId));
        var versions = entity.Versions.Select(
            v =>
            {
                var id = Guard.NotNull(v.Id).Value;
                return new ResourceLink(id.Value, linkGenerator.GetContractVersion(id));
            }).ToArray();

        return new ContractModel(
            Id: entity.Id!.Value.Value,
            Name: entity.Name,
            Workspace: workspace,
            Versions: versions
            );
    }
}

