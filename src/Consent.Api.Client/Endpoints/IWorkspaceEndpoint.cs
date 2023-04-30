using System.Threading.Tasks;
using Consent.Api.Client.Models.Workspaces;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IWorkspaceEndpoint
{
    [Get("/workspace/{id}")]
    Task<WorkspaceModel> WorkspaceGet(int id, [Header("userId")] int userId);

    [Post("/workspace")]
    Task<WorkspaceModel> WorkspaceCreate([Body] WorkspaceCreateRequestModel request, [Header("userId")] int userId);
}
