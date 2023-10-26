using System;
using System.Linq;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Contracts;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Permissions;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Consent.Storage.Contracts;
using Consent.Storage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Contracts;

[ApiController]
[Route("[controller]")]
public class ContractController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<ContractController> _logger;
    private readonly LinkGenerator _linkGenerator;
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;
    private readonly ContractCreateRequestModelValidator _contractValidator = new();
    private readonly ContractVersionCreateRequestModelValidator _contractVersionValidator = new();
    private readonly ProvisionCreateRequestModelValidator _provisionValidator = new();

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public ContractController(
        ILogger<ContractController> logger, LinkGenerator linkGenerator,
        IContractRepository contractRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _contractRepository = contractRepository;
        _userRepository = userRepository;
    }

    [HttpGet("{id}", Name = "GetContract")]
    public async Task<ActionResult<ContractModel>> ContractGet(
        int id,
        [FromHeader] int userId)
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
        return Ok(model);
    }

    [HttpPost("", Name = "CreateContract")]
    public async Task<ActionResult<ContractModel>> ContractCreate(ContractCreateRequestModel request, [FromHeader] int userId)
    {
        var validationResult = _contractValidator.Validate(request);
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

    [HttpGet("{contractId}/version/{id}", Name = "GetContractVersion")]
    public async Task<ActionResult<ContractVersionModel>> ContractVersionGet(int contractId, int id, [FromHeader] int userId)
    {
        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return Forbid();
        }

        var contract = await _contractRepository.Get(new ContractId(contractId));
        if (contract == null)
        {
            return NotFound();
        }

        if (!UserHasPermissions(user, contract.WorkspaceId, WorkspacePermission.View))
        {
            return NotFound();
        }

        var versionId = new ContractVersionId(id);
        var version = contract.Versions.SingleOrDefault(v => v.Id == versionId);

        if (version == null)
        {
            return NotFound();
        }

        var model = version.ToModel(contract, Links);
        return Ok(model);
    }

    [HttpPost("{contractId}/version", Name = "CreateContractVersion")]
    public async Task<ActionResult<ContractVersionModel>> ContractVersionCreate(
        [FromRoute] int contractId, ContractVersionCreateRequestModel request, [FromHeader] int userId)
    {
        var validationResult = _contractVersionValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return Forbid();
        }

        var contract = await _contractRepository.Get(new ContractId(contractId));
        if (contract == null)
        {
            return Forbid();
        }

        if (!UserHasPermissions(user, contract.WorkspaceId, WorkspacePermission.View))
        {
            return Forbid();
        }

        var created = new ContractVersion(
            Guard.NotNull(request.Name), Guard.NotNull(request.Text), Array.Empty<Provision>()
            );
        contract.AddContractVersions(created);

        await _contractRepository.Update(contract);

        return Ok(created.ToModel(contract, Links));
    }

    [HttpPost("{contractId}/version/{versionId}/provision", Name = "CreateProvision")]
    public async Task<ActionResult<ProvisionModel>> ProvisionCreate(
        [FromRoute] int contractId, [FromRoute] int versionId, ProvisionCreateRequestModel request, [FromHeader] int userId)
    {
        var validationResult = _provisionValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return Forbid();
        }

        var contract = await _contractRepository.Get(new ContractId(contractId));
        if (contract == null)
        {
            return NotFound();
        }

        if (!UserHasPermissions(user, contract.WorkspaceId, WorkspacePermission.View))
        {
            return NotFound();
        }

        var versionIdent = new ContractVersionId(versionId);
        var version = contract.Versions.SingleOrDefault(v => v.Id == versionIdent);
        if (version == null)
        {
            return NotFound();
        }

        var permissionIds = Guard.NotNull(request.PermissionIds).Select(pid => new PermissionId(pid));
        // todo validate permissions actually exist
        var created = new Provision(Guard.NotNull(request.Text), permissionIds);
        version.AddProvisions(created);

        await _contractRepository.Update(contract);

        return Ok(created.ToModel(version, contract, Links));
    }

    private bool UserHasPermissions(User user, WorkspaceId workspaceId, WorkspacePermission requiredPermission)
    {
        var membership = user.WorkspaceMemberships.SingleOrDefault(m => m.WorkspaceId == workspaceId);
        if (membership == null || !membership.Permissions.Contains(requiredPermission))
        {
            return false;
        }

        return true;
    }
}
