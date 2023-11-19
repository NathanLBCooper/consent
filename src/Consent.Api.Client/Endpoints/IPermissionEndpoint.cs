using System.Threading.Tasks;
using Consent.Api.Client.Models.Permissions;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IPermissionEndpoint
{
    [Get("/permission/{id}")]
    Task<PermissionModel> PermissioGet(int id, [Header("userId")] int userId);

    [Post("/permission")]
    Task<PermissionModel> PermissionCreate([Body] PermissionCreateRequestModel request, [Header("userId")] int userId);
}
