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

    [Get("/contract/version/{id}")]
    Task<ContractVersionModel> ContractVersionGet(int id, [Header("userId")] int userId);

    [Post("/contract/{contractId}/version")]
    Task<ContractVersionModel> ContractVersionCreate(
        int contractId, [Body] ContractVersionCreateRequestModel request, [Header("userId")] int userId);

    [Post("/contract/version/{versionId}/provision")]
    Task<ProvisionModel> ProvisionCreate(int versionId, [Body] ProvisionCreateRequestModel request, [Header("userId")] int userId);
}
