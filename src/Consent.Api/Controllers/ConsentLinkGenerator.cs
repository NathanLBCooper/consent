using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Controllers;

public class ConsentLinkGenerator
{
    private readonly HttpContext _httpContext;
    private readonly LinkGenerator _linkGenerator;

    public ConsentLinkGenerator(HttpContext httpContext, LinkGenerator linkGenerator)
    {
        _httpContext = httpContext;
        _linkGenerator = linkGenerator;
    }

    public string? GetWorkspace(int workspaceId)
    {
        return _linkGenerator.GetPathByAction(_httpContext,
            action: nameof(WorkspaceController.WorkspaceGet),
            controller: "Workspace",
            values: new { Id = workspaceId }
            );
    }
}
