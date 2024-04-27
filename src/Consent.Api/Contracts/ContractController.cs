using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Contracts;
using Consent.Application.Contracts.Create;
using Consent.Application.Contracts.Get;
using Consent.Application.Contracts.ProvisionCreate;
using Consent.Application.Contracts.VersionCreate;
using Consent.Application.Contracts.VersionGet;
using Consent.Domain.Contracts;
using Consent.Domain.Purposes;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Contracts;

[ApiController]
[Route("[controller]")]
public class ContractController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IContractGetQueryHandler _contractGet;
    private readonly IContractCreateCommandHandler _contractCreate;
    private readonly IContractVersionGetQueryHandler _versionGet;
    private readonly IContractVersionCreateCommandHandler _versionCreate;
    private readonly IProvisionCreateCommandHandler _provisionCreate;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public ContractController(
        LinkGenerator linkGenerator,
        IContractGetQueryHandler contractGet, IContractCreateCommandHandler contractCreate,
        IContractVersionGetQueryHandler versionGet, IContractVersionCreateCommandHandler versionCreate,
        IProvisionCreateCommandHandler provisionCreate)
    {
        _linkGenerator = linkGenerator;
        _contractGet = contractGet;
        _contractCreate = contractCreate;
        _versionGet = versionGet;
        _versionCreate = versionCreate;
        _provisionCreate = provisionCreate;
    }

    [HttpGet("{id}", Name = "GetContract")]
    public async Task<ActionResult<ContractModel>> ContractGet(int id, [FromHeader] int userId,
        CancellationToken cancellationToken)
    {
        var query = new ContractGetQuery(new ContractId(id), new UserId(userId));
        var result = await _contractGet.Handle(query, cancellationToken);

        if (result.Unwrap() is not { } contract)
        {
            return NotFound();
        }

        return Ok(contract.ToModel(Links));
    }

    [HttpPost("", Name = "CreateContract")]
    public async Task<ActionResult<ContractModel>> ContractCreate(ContractCreateRequestModel request,
        [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new ContractCreateCommand(request.Name, new WorkspaceId(request.WorkspaceId), new UserId(userId));
        var contract = await _contractCreate.Handle(command, cancellationToken);
        return Ok(contract.ToModel(Links));
    }

    [HttpGet("version/{id}", Name = "GetContractVersion")]
    public async Task<ActionResult<ContractVersionModel>> ContractVersionGet(int id, [FromHeader] int userId,
        CancellationToken cancellationToken)
    {
        var query = new ContractVersionGetQuery(new ContractVersionId(id), new UserId(userId));
        var maybe = await _versionGet.Handle(query, cancellationToken);
        if (maybe.Unwrap() is not ContractVersionGetQueryResult result)
        {
            return NotFound();
        }

        return Ok(result.Version.ToModel(result.Contract, Links));
    }

    [HttpPost("{contractId}/version", Name = "CreateContractVersion")]
    public async Task<ActionResult<ContractVersionModel>> ContractVersionCreate(
        [FromRoute] int contractId, ContractVersionCreateRequestModel request, [FromHeader] int userId,
        CancellationToken cancellationToken)
    {
        var command = new ContractVersionCreateCommand(request.Name, request.Text, new ContractId(contractId),
            new UserId(userId));
        var result = await _versionCreate.Handle(command, cancellationToken);

        return Ok(result.Version.ToModel(result.Contract, Links));
    }

    [HttpPost("version/{versionId}/provision", Name = "CreateProvision")]
    public async Task<ActionResult<ProvisionModel>> ProvisionCreate(
        [FromRoute] int versionId, ProvisionCreateRequestModel request, [FromHeader] int userId,
        CancellationToken cancellationToken)
    {
        var command = new ProvisionCreateCommand(
            request.Text, request.PurposeIds?.Select(p => new PurposeId(p)).ToArray(), new ContractVersionId(versionId),
            new UserId(userId)
        );
        var result = await _provisionCreate.Handle(command, cancellationToken);

        return Ok(result.Provision.ToModel(result.Version, Links));
    }
}
