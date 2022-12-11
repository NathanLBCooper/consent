using System.Threading.Tasks;
using Consent.Api.Models;
using Consent.Domain.UnitOfWork;
using Consent.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ICreateUnitOfWork _createUnitOfWork;
    private readonly UserCreateRequestModelValidator _userCreateRequestModelValidator = new();
    private readonly EtagHelper _etagHelper;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, ICreateUnitOfWork createUnitOfWork)
    {
        _logger = logger;
        _userRepository = userRepository;
        _createUnitOfWork = createUnitOfWork;
        _etagHelper = new EtagHelper();
    }

    [HttpGet("", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet(
        [FromHeader] int userId, [FromHeader(Name = HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch)
    {
        using var uow = _createUnitOfWork.Create();
        var user = await _userRepository.Get(new UserId(userId));

        if (user == null)
        {
            return NotFound();
        }

        var model = user.ToModel();
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

        using var uow = _createUnitOfWork.Create();
        var created = await _userRepository.Create(new User(request.Name));
        await uow.CommitAsync();

        var model = created.ToModel();
        var etag = _etagHelper.Get("user", model);

        Response.Headers.Add(HttpHeaderNames.ETag, etag);
        return Ok(model);
    }
}
