using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consent.Api.Controllers;
using Consent.Api.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Tests.Builders;
internal class UserControllerTestWrapper
{
    private readonly UserController _controller;

    public UserControllerTestWrapper(UserController controller)
        : base()
    {
        _controller = controller;
    }

    public async Task<ActionResult<UserModel>> UserCreate(UserCreateRequestModel request)
    {
        _controller.HttpContext.Request.Headers.Clear();
        _controller.HttpContext.Response.Headers.Clear();
        var result = await _controller.UserCreate(request);
        _controller.HttpContext.Request.Headers.Clear();

        return result;
    }

    public async Task<ActionResult<UserModel>> UserGet(int userId, string? ifNoneMatch)
    {
        _controller.HttpContext.Request.Headers.Clear();
        _controller.HttpContext.Response.Headers.Clear();
        var result = await _controller.UserGet(userId, ifNoneMatch);
        _controller.HttpContext.Request.Headers.Clear();

        return result;
    }

    public IDictionary<string, string> LastResponseHeaders =>
        _controller.ControllerContext.HttpContext.Response.Headers
        .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
}

