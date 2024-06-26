using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Consent.Application.Users.Create;
using Consent.Application.Users.Get;
using Consent.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Consent.Api.Users;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserGetQueryHandler _get;
    private readonly IUserCreateCommandHandler _create;

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public UserController(LinkGenerator linkGenerator, IUserGetQueryHandler get, IUserCreateCommandHandler create)
    {
        _linkGenerator = linkGenerator;
        _get = get;
        _create = create;
    }

    [HttpGet("", Name = "GetUserSelf")]
    public async Task<ActionResult<UserModel>> UserGet([FromHeader] int userId, CancellationToken cancellationToken)
    {
        var id = new UserId(userId);
        var command = new UserGetQuery(id, id);
        var maybe = await _get.Handle(command, cancellationToken);

        if (maybe.Value is not { } user)
        {
            return NotFound();
        }

        return Ok(user.ToModel(Links));
    }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet(int id, [FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new UserGetQuery(new UserId(id), new UserId(userId));
        var maybe = await _get.Handle(command, cancellationToken);

        if (maybe.Value is not { } user)
        {
            return NotFound();
        }

        return Ok(user.ToModel(Links));
    }

    [HttpPost("", Name = "CreateUser")]
    public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request, CancellationToken cancellationToken)
    {
        var command = new UserCreateCommand(request.Name);
        var result = await _create.Handle(command, cancellationToken);

        if (result.Value is not { } user)
        {
            return result.UnwrapError().ToErrorResponse<UserModel>(this);
        }

        return Ok(user.ToModel(Links));
    }
}
