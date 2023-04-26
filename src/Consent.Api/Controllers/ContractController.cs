using System.Linq;
using System.Threading.Tasks;
using Consent.Api.Models.Contracts;
using Consent.Domain;
using Consent.Domain.Contracts;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContractController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<ContractController> _logger;
    private readonly LinkGenerator _linkGenerator;
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;
    private readonly EtagHelper _etagHelper;
    private readonly ContractCreateRequestModelValidator _validator = new();

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public ContractController(ILogger<ContractController> logger, LinkGenerator linkGenerator, IContractRepository contractRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _contractRepository = contractRepository;
        _userRepository = userRepository;
        _etagHelper = new EtagHelper();
    }

    [HttpGet("", Name = "GetContract")]
    public async Task<ActionResult<ContractModel>> ContractGet(
        int id,
        [FromHeader] int userId,
        [FromHeader(Name = HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch)
    {
        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return Forbid();
        }

        var contract = await _contractRepository.Get(new ContractId(id));
        if (contract == null)
        {
            return NotFound();
        }

        if (!UserHasPermissions(user, contract.WorkspaceId, WorkspacePermission.View))
        {
            return NotFound();
        }

        var model = contract.ToModel(Links);
        var etag = _etagHelper.Get("contract", model);

        Response.Headers.Add(HttpHeaderNames.ETag, etag);
        if (ifNoneMatch != null && etag == ifNoneMatch)
        {
            return StatusCode(304);
        }

        return Ok(model);
    }

    [HttpPost("", Name = "CreateContract")]
    public async Task<ActionResult<ContractModel>> ContractCreate(ContractCreateRequestModel request, [FromHeader] int userId)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return Forbid();
        }

        var workspaceId = new WorkspaceId(request.WorkspaceId);
        if (!UserHasPermissions(user, workspaceId, WorkspacePermission.Edit))
        {
            return Forbid();
        }

        var entity = await _contractRepository.Create(new Contract(Guard.NotNull(request.Name), workspaceId));

        return Ok(entity.ToModel(Links));
    }

    [HttpGet("version", Name = "GetContractVersion")]
    public ActionResult<string> ContractVersionGet(
    int id,
    [FromHeader] int userId,
    [FromHeader(Name = HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch)
    {
        return Problem();
    }

    private bool UserHasPermissions(User user, WorkspaceId workspaceId, WorkspacePermission requiredPermission)
    {
        var membership = user.WorkspaceMemberships.SingleOrDefault(m => m.WorkspaceId == workspaceId);
        if (membership == null || !membership.Permissions.Contains(WorkspacePermission.Edit))
        {
            return false;
        }

        return true;
    }
}

