using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Workspaces;
using Consent.Application.Workspaces;
using Consent.Domain.Core;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Workspaces;

[ApiController]
[Route("[controller]")]
public class WorkspaceController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly IWorkspaceGetQueryHandler _get;
    private readonly IWorkspaceCreateCommandHandler _create;

    public WorkspaceController(IWorkspaceGetQueryHandler get, IWorkspaceCreateCommandHandler create)
    {
        _get = get;
        _create = create;
    }

    [HttpGet("{id}", Name = "GetWorkspace")]
    public async Task<ActionResult<WorkspaceModel>> WorkspaceGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var query = new WorkspaceGetQuery(new WorkspaceId(id), new UserId(userId));
        var maybe = await _get.Handle(query, cancellationToken);
        return maybe.Match<Workspace, ActionResult<WorkspaceModel>>(
            workspace => Ok(workspace.ToModel()),
            () => NotFound()
            );
    }

    [HttpPost("", Name = "CreateWorkspace")]
    public async Task<ActionResult<WorkspaceModel>> WorkspaceCreate(WorkspaceCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new WorkspaceCreateCommand(request.Name, new UserId(userId));
        var result = await _create.Handle(command, cancellationToken);
        return result.Match(
            workspace => Ok(workspace.ToModel()),
            error => error.ToErrorResponse<WorkspaceModel>(this)
            );
    }
}
