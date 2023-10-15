using Consent.Api.Contracts;
using Consent.Api.Workspaces;
using Consent.Domain.Contracts;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api;

internal class ConsentLinkGenerator
{
    private readonly HttpContext _httpContext;
    private readonly LinkGenerator _linkGenerator;

    public ConsentLinkGenerator(HttpContext httpContext, LinkGenerator linkGenerator)
    {
        _httpContext = httpContext;
        _linkGenerator = linkGenerator;
    }

    public string? GetWorkspace(WorkspaceId workspaceId)
    {
        return _linkGenerator.GetPathByAction(_httpContext,
            action: nameof(WorkspaceController.WorkspaceGet),
            controller: "Workspace",
            values: new { Id = workspaceId.Value }
            );
    }

    public string? GetContract(ContractId contractId)
    {
        return _linkGenerator.GetPathByAction(_httpContext,
                    action: nameof(ContractController.ContractGet),
                    controller: "contract",
                    values: new { Id = contractId }
                    );
    }

    public string? GetContractVersion(ContractId contractId, ContractVersionId contractVersionId)
    {
        return _linkGenerator.GetPathByAction(_httpContext,
            action: nameof(ContractController.ContractVersionGet),
            controller: "contract",
            values: new { ContractId = contractId, Id = contractVersionId.Value }
            );
    }
}
