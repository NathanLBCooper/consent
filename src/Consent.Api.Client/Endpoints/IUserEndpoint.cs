using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Refit;

namespace Consent.Api.Client.Endpoints;
public interface IUserEndpoint
{
    [Get("/user")]
    Task<UserModel> UserGet([Header("userId")] int userId);
    [Get("/user")]
    Task<ApiResponse<UserModel>> UserGetReq([Header("userId")] int userId, [Header(HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch);

    [Post("/user")]
    Task<UserModel> UserCreate([Body] UserCreateRequestModel request);
    [Post("/user")]
    Task<ApiResponse<UserModel>> UserCreateReq([Body] UserCreateRequestModel request);
}
