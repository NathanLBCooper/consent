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

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, ICreateUnitOfWork createUnitOfWork)
    {
        _logger = logger;
        _userRepository = userRepository;
        _createUnitOfWork = createUnitOfWork;
    }

    [HttpGet("", Name = "GetUser")]
    public async Task<ActionResult<UserModel>> UserGet([FromHeader] int userId)
    {
        using var uow = _createUnitOfWork.Create();
        var user = await _userRepository.Get(new UserId(userId));

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.ToModel());
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

        return Ok(created.ToModel());
    }
}
