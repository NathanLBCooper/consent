using System.Linq;
using System.Threading.Tasks;
using Consent.Api.Models;
using Consent.Domain.UnitOfWork;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkspaceController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<WorkspaceController> _logger;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICreateUnitOfWork _createUnitOfWork;
    private readonly WorkspaceCreateRequestModelValidator _workspaceCreateRequestModelValidator = new();

    public WorkspaceController(ILogger<WorkspaceController> logger,
        IWorkspaceRepository workspaceRepository, IUserRepository userRepository,
        ICreateUnitOfWork createUnitOfWork)
    {
        _logger = logger;
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
        _createUnitOfWork = createUnitOfWork;
    }

    [HttpGet("{id}", Name = "GetWorkspace")]
    public async Task<ActionResult<WorkspaceModel>> WorkspaceGet(int id, [FromHeader] int userId)
    {
        var workspaceId = new WorkspaceId(id);
        var userIdentity = new UserId(userId);

        using var uow = _createUnitOfWork.Create();

        var workspace = await _workspaceRepository.Get(workspaceId);
        if (workspace == null || !workspace.GetUserPermissions(userIdentity).Contains(WorkspacePermission.View))
        {
            return NotFound();
        }

        return Ok(workspace.ToModel());
    }

    [HttpPost("", Name = "CreateWorkspace")]
    public async Task<ActionResult<WorkspaceModel>> WorkspaceCreate(WorkspaceCreateRequestModel request, [FromHeader] int userId)
    {
        var validationResult = _workspaceCreateRequestModelValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        if (request?.Name == null)
        {
            return Problem();
        }

        using var uow = _createUnitOfWork.Create();

        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return NotFound();
        }

        var entity = await _workspaceRepository.Create(new Workspace(request.Name, user.Id));

        await uow.CommitAsync();

        return Ok(entity.ToModel());
    }
}
