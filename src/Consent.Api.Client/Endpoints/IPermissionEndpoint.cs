using System.Threading.Tasks;
using Consent.Api.Client.Models.Purposes;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IPurposeEndpoint
{
    [Get("/purpose/{id}")]
    Task<PurposeModel> PurposeGet(int id, [Header("userId")] int userId);

    [Post("/purpose")]
    Task<PurposeModel> PurposeCreate([Body] PurposeCreateRequestModel request, [Header("userId")] int userId);
}
