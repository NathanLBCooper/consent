using Consent.Api.Models;
using Consent.Domain;
using Consent.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Consent.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase // [FromHeader] int userId is honestly based auth
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserEndpoint _userEndpoint;
        private readonly UserCreateRequestModelValidator _userCreateRequestModelValidator = new();

        public UserController(ILogger<UserController> logger, IUserEndpoint userEndpoint)
        {
            _logger = logger;
            _userEndpoint = userEndpoint;
        }

        [HttpGet("User", Name = "GetUser")]
        public async Task<ActionResult<UserModel>> UserGet([FromHeader] int userId)
        {
            var user = await _userEndpoint.UserGet(new Context { UserId = userId });

            if (user == null) return NotFound();

            return Ok(user.ToModel());
        }

        [HttpPost("User", Name = "CreateUser")]
        public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request)
        {
            var validationResult = _userCreateRequestModelValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.ToString());
            }

            if (request?.Name == null) return Problem();

            var user = await _userEndpoint.UserCreate(new User(request.Name));

            return Ok(user.ToModel());
        }
    }
}
