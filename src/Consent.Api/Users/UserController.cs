using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Consent.Domain.Core;
using Consent.Domain.Users;
using Consent.Storage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Consent.Api.Users;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase // [FromHeader] int userId is honestly based auth
{
    private readonly ILogger<UserController> _logger;
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserRepository _userRepository;
    private readonly UserCreateRequestModelValidator _validator = new();

    private ConsentLinkGenerator Links => new(HttpContext, _linkGenerator);

    public UserController(ILogger<UserController> logger, LinkGenerator linkGenerator, IUserRepository userRepository)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _userRepository = userRepository;
    }

    [HttpGet("", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet([FromHeader] int userId)
    {
        var user = await _userRepository.Get(new UserId(userId));
        if (user == null)
        {
            return NotFound();
        }

        var model = user.ToModel(Links);
        return Ok(model);
    }

    [HttpPost("", Name = "CreateUser")]
    public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.ToString());
        }

        var created = await _userRepository.Create(new User(Guard.NotNull(request.Name)));

        var model = created.ToModel(Links);
        return Ok(model);
    }
}
