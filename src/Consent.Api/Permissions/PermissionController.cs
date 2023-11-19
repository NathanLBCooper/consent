using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Permissions;

[ApiController]
[Route("[controller]")]
public class PermissionController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public PermissionController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet("{id}", Name = "GetPermission")]
    public async Task<ActionResult<PermissionModel>> PermissionGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        _ = id;
        _ = userId;
        _ = cancellationToken;
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }

    [HttpPost("", Name = "CreatePermission")]
    public async Task<ActionResult<PermissionModel>> PermissionCreate(PermissionCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        _ = request;
        _ = userId;
        _ = cancellationToken;
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }
}
