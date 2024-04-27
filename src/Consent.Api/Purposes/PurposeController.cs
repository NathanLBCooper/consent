using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Purposes;
using Consent.Application.Purposes.Create;
using Consent.Application.Purposes.Get;
using Consent.Domain.Purposes;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Purposes;

[ApiController]
[Route("[controller]")]
public class PurposeController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IPurposeGetQueryHandler _get;
    private readonly IPurposeCreateCommandHandler _create;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public PurposeController(LinkGenerator linkGenerator, IPurposeGetQueryHandler get,
        IPurposeCreateCommandHandler create)
    {
        _linkGenerator = linkGenerator;
        _get = get;
        _create = create;
    }

    [HttpGet("{id}", Name = "GetPurpose")]
    public async Task<ActionResult<PurposeModel>> PurposeGet(int id, [FromHeader] int userId,
        CancellationToken cancellationToken)
    {
        var query = new PurposeGetQuery(new PurposeId(id), new UserId(userId));
        var maybe = await _get.Handle(query, cancellationToken);

        if (maybe.Value is not { } purpose)
        {
            return NotFound();
        }

        return Ok(purpose.ToModel(Links));
    }

    [HttpPost("", Name = "CreatePurpose")]
    public async Task<ActionResult<PurposeModel>> PurposeCreate(
        PurposeCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new PurposeCreateCommand(
            request.Name, request.Description, new WorkspaceId(request.WorkspaceId), new UserId(userId)
        );
        var result = await _create.Handle(command, cancellationToken);

        if (result.Value is not { } purpose)
        {
            return result.UnwrapError().ToErrorResponse<PurposeModel>(this);
        }

        return Ok(purpose.ToModel(Links));
    }
}
