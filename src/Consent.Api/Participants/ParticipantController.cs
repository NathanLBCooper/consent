using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Participants;
using Consent.Application.Participants.Create;
using Consent.Application.Participants.Get;
using Consent.Domain.Participants;
using Consent.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Participants;

[ApiController]
[Route("[controller]")]
public class ParticipantController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IParticipantGetQueryHandler _get;
    private readonly IParticipantCreateCommandHandler _create;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public ParticipantController(LinkGenerator linkGenerator, IParticipantGetQueryHandler get, IParticipantCreateCommandHandler create)
    {
        _linkGenerator = linkGenerator;
        _get = get;
        _create = create;
    }

    [HttpGet("{id}", Name = "GetParticipant")]
    public async Task<ActionResult<ParticipantModel>> ParticipantGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var query = new ParticipantGetQuery(new ParticipantId(id), new UserId(userId));
        var maybe = await _get.Handle(query, cancellationToken);

        if (maybe.Value is not { } participant)
        {
            return NotFound();
        }

        return Ok(participant.ToModel(Links));
    }

    [HttpPost("", Name = "CreateParticipant")]
    public async Task<ActionResult<ParticipantModel>> ParticipantCreate(
        ParticipantCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var _ = request;
        var command = new ParticipantCreateCommand(new UserId(userId));
        var result = await _create.Handle(command, cancellationToken);

        if (result.Value is not { } participant)
        {
            return result.UnwrapError().ToErrorResponse<ParticipantModel>(this);
        }

        return Ok(participant.ToModel(Links));
    }
}
