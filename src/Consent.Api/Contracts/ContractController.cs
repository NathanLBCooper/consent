using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Contracts;
using Consent.Application.Contracts;
using Consent.Application.Users;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Permissions;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
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
    private readonly ContractVersionCreateRequestModelValidator _contractVersionValidator = new();
    private readonly ProvisionCreateRequestModelValidator _provisionValidator = new();
    private readonly IContractGetQueryHandler _get;
    private readonly IContractCreateCommandHandler _create;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public ContractController(
        ILogger<ContractController> logger, LinkGenerator linkGenerator,
        IContractRepository contractRepository, IUserRepository userRepository,
        IContractGetQueryHandler get, IContractCreateCommandHandler create)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _contractRepository = contractRepository;
        _userRepository = userRepository;
        _get = get;
        _create = create;
    }

    [HttpGet("{id}", Name = "GetContract")]
    public async Task<ActionResult<ContractModel>> ContractGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var query = new ContractGetQuery(new ContractId(id), new UserId(userId));
        var maybe = await _get.Handle(query, cancellationToken);
        return maybe.Match<Contract, ActionResult<ContractModel>>(
            contract => Ok(contract.ToModel(Links)),
            () => NotFound()
            );
    }

    [HttpPost("", Name = "CreateContract")]
    public async Task<ActionResult<ContractModel>> ContractCreate(ContractCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new ContractCreateCommand(request.Name, new WorkspaceId(request.WorkspaceId), new UserId(userId));
        var result = await _create.Handle(command, cancellationToken);
        return result.Match(
            contract => Ok(contract.ToModel(Links)),
            error => error.ToErrorResponse<ContractModel>(this)
            );
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
