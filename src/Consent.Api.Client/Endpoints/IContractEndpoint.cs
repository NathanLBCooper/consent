using System.Threading.Tasks;
using Consent.Api.Client.Models.Contracts;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IContractEndpoint
{
    [Get("/contract/{id}")]
    Task<ContractModel> ContractGet(int id, [Header("userId")] int userId);

    [Post("/contract")]
    Task<ContractModel> ContractCreate([Body] ContractCreateRequestModel request, [Header("userId")] int userId);

    [Get("/contract/{contractId}/version/{id}")]
    Task<ContractVersionModel> ContractVersionGet(int contractId, int id, [Header("userId")] int userId);

    [Post("/contract/{contractId}/version")]
    Task<ContractVersionModel> ContractVersionCreate(
        int contractId, [Body] ContractVersionCreateRequestModel request, [Header("userId")] int userId);
}
