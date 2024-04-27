using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Workspaces;
using Consent.Application.Workspaces.Create;
using Consent.Application.Workspaces.Get;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;

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

        if (maybe.Value is not { } workspace)
        {
            return NotFound();
        }

        return Ok(workspace.ToModel());
    }

    [HttpPost("", Name = "CreateWorkspace")]
    public async Task<ActionResult<WorkspaceModel>> WorkspaceCreate(WorkspaceCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new WorkspaceCreateCommand(request.Name, new UserId(userId));
        var result = await _create.Handle(command, cancellationToken);

        if (result.Value is not { } workspace)
        {
            return result.UnwrapError().ToErrorResponse<WorkspaceModel>(this);
        }

        return Ok(workspace.ToModel());
    }
}
