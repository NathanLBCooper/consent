using Consent.Api.Models;
using Consent.Domain;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Consent.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkspaceController : ControllerBase // [FromHeader] int userId is honestly based auth
    {
        private readonly ILogger<WorkspaceController> _logger;
        private readonly IWorkspaceEndpoint _workspaceEndpoint;
        private readonly UserCreateRequestModelValidator _userCreateRequestModelValidator = new();
        private readonly WorkspaceCreateRequestModelValidator _workspaceCreateRequestModelValidator = new();

        public WorkspaceController(ILogger<WorkspaceController> logger, IWorkspaceEndpoint workspaceEndpoint)
        {
            _logger = logger;
            _workspaceEndpoint = workspaceEndpoint;
        }

        [HttpGet("Workspace/{id}", Name = "GetWorkspace")]
        public async Task<ActionResult<WorkspaceModel>> WorkspaceGet(int id, [FromHeader] int userId)
        {
            var workspace = await _workspaceEndpoint.WorkspaceGet(new WorkspaceId(id), new Context { UserId = new UserId(userId) });

            if (workspace == null) return NotFound();

            return Ok(workspace.ToModel());
        }

        [HttpPost("Workspace", Name ="CreateWorkspace")]
        public async Task<ActionResult<WorkspaceModel>> WorkspaceCreate(WorkspaceCreateRequestModel request, [FromHeader] int userId)
        {
            var validationResult = _workspaceCreateRequestModelValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.ToString());
            }

            if (request?.Name == null) return Problem();

            var workspace = await _workspaceEndpoint.WorkspaceCreate(new Workspace(request.Name), new Context { UserId = new UserId(userId) });

            return Ok(workspace.ToModel());
        }

        [HttpGet("Workspace/{id}/Permissions", Name = "PermissionsGet")]
        public async Task<ActionResult<WorkspacePermission[]>> PermissionsGet(int workspaceId, [FromHeader] int userId)
        {
            var permissions = await _workspaceEndpoint.WorkspacePermissionsGet(new WorkspaceId(workspaceId), new Context { UserId = new UserId(userId) });

            if (permissions == null || !permissions.Any()) return NotFound();

            return Ok(permissions);
        }
    }
}
