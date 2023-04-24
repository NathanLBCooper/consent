using System.Threading.Tasks;
using Consent.Api.Models;
using Consent.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<UserController> _logger;
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserRepository _userRepository;
    private readonly UserCreateRequestModelValidator _userCreateRequestModelValidator = new();
    private readonly EtagHelper _etagHelper;

    public UserController(ILogger<UserController> logger, LinkGenerator linkGenerator, IUserRepository userRepository)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _userRepository = userRepository;
        _etagHelper = new EtagHelper();
    }

    [HttpGet("", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet(
        [FromHeader] int userId, [FromHeader(Name = HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch)
    {
        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return NotFound();
        }

        var model = user.ToModel(Links);
        var etag = _etagHelper.Get("user", model);

        if (ifNoneMatch != null && etag == ifNoneMatch)
        {
            return StatusCode(304);
        }

        Response.Headers.Add(HttpHeaderNames.ETag, etag);
        return Ok(model);
    }

    [HttpPost("", Name = "CreateUser")]
    public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request)
    {
        var validationResult = _userCreateRequestModelValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        if (request?.Name == null)
        {
            return Problem();
        }

        var created = await _userRepository.Create(new User(request.Name));

        var model = created.ToModel(Links);
        var etag = _etagHelper.Get("user", model);

        Response.Headers.Add(HttpHeaderNames.ETag, etag);
        return Ok(model);
    }

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);
}
