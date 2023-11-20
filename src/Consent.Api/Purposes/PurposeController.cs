using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Purposes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Purposes;

[ApiController]
[Route("[controller]")]
public class PurposeController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public PurposeController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet("{id}", Name = "GetPurpose")]
    public async Task<ActionResult<PurposeModel>> PurposeGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        _ = id;
        _ = userId;
        _ = cancellationToken;
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }

    [HttpPost("", Name = "CreatePurpose")]
    public async Task<ActionResult<PurposeModel>> PurposeCreate(PurposeCreateRequestModel request, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        _ = request;
        _ = userId;
        _ = cancellationToken;
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }
}
