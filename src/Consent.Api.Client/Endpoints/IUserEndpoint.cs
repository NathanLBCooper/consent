using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IUserEndpoint
{
    [Get("/user")]
    Task<UserModel> UserGet([Header("userId")] int userId);

    [Get("/user/{id}")]
    Task<UserModel> UserGet(int id, [Header("userId")] int userId);

    [Post("/user")]
    Task<UserModel> UserCreate([Body] UserCreateRequestModel request);
}
