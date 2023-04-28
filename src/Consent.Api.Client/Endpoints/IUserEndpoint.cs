using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Refit;

namespace Consent.Api.Client.Endpoints;
public interface IUserEndpoint
{
    [Get("/users/{userId}")]
    Task<UserModel> UserGet(int userId, [Header(HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch);

    [Post("/users")]
    Task<UserModel> UserCreate(UserCreateRequestModel request);
}
