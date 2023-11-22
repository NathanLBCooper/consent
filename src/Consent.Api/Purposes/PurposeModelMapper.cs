using Consent.Api.Client.Models;
using Consent.Api.Client.Models.Purposes;
using Consent.Domain.Core;
using Consent.Domain.Purposes;

namespace Consent.Api.Purposes;

internal static class PurposeModelMapper
{
    public static PurposeModel ToModel(this Purpose entity, ConsentLinkGenerator linkGenerator)
    {
        var workspace = new ResourceLink(entity.WorkspaceId.Value, linkGenerator.GetWorkspace(entity.WorkspaceId));

        var id = Guard.NotNull(entity.Id).Value;
        return new PurposeModel(id, entity.Name, entity.Description, workspace);
    }
}
