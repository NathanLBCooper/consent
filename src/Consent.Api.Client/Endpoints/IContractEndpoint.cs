using System.Threading.Tasks;
using Consent.Api.Client.Models.Contracts;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IContractEndpoint
{
    [Get("/contract/{id}")]
    Task<ContractModel> ContractGet(int id, [Header("userId")] int userId);
    [Get("/contract/{id}")]
    Task<ApiResponse<ContractModel>> ContractGetReq(int id, [Header("userId")] int userId, [Header(HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch);

    [Post("/contract")]
    Task<ContractModel> ContractCreate([Body] ContractCreateRequestModel request, [Header("userId")] int userId);
    [Post("/contract")]
    Task<ApiResponse<ContractModel>> ContractCreateReq([Body] ContractCreateRequestModel request, [Header("userId")] int userId);

    [Get("/contract/{contractId}/version/{id}")]
    Task<ContractModel> ContractVersionGet(int contractId, int id, [Header("userId")] int userId);
    [Get("/contract/{contractId}/version/{id}")]
    Task<ApiResponse<ContractModel>> ContractVersionGetReq(int contractId, int id, [Header("userId")] int userId, [Header(HttpHeaderNames.IfNoneMatch)] string? ifNoneMatch);
}
