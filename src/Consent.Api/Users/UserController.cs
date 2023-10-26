using System.Threading;
using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Consent.Application.Users;
using Consent.Domain.Core;
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

    [HttpGet("", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet([FromHeader] int userId, CancellationToken cancellationToken)
    {
        var command = new UserGetQuery(new UserId(userId));
        var maybe = await _get.Handle(command, cancellationToken);

        var response = maybe.Match<User, ActionResult<UserModel>>(
            user => Ok(user.ToModel(Links)),
            () => NotFound()
        );

        return response;
    }

    [HttpPost("", Name = "CreateUser")]
    public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request, CancellationToken cancellationToken)
    {
        var command = new UserCreateCommand(request.Name);
        var result = await _create.Handle(command, cancellationToken);

        var response = result.Match(
            user => Ok(user.ToModel(Links)),
            error => error.ToErrorResponse<UserModel>(this)
            );

        return response;
    }
}
