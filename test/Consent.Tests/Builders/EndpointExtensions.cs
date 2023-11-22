using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Contracts;
using Consent.Api.Client.Models.Purposes;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;

namespace Consent.Tests.Builders;

/// <summary>
/// Call endpoints with default data from builders
/// </summary>
internal static class EndpointExtensions
{
    public static async Task<UserModel> UserCreate(IUserEndpoint endpoint)
    {
        return await endpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
    }

    public static async Task<WorkspaceModel> WorkspaceCreate(IWorkspaceEndpoint endpoint, UserModel user)
    {
        return await endpoint.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), user.Id);
    }

    public static async Task<ContractModel> ContractCreate(IContractEndpoint endpoint, WorkspaceModel workspace, UserModel user)
    {
        return await endpoint.ContractCreate(new ContractCreateRequestModelBuilder(workspace.Id).Build(), user.Id);
    }

    public static async Task<ContractVersionModel> VersionCreate(IContractEndpoint endpoint, ContractModel contract, UserModel user)
    {
        return await endpoint.ContractVersionCreate(
            contract.Id, new ContractVersionCreateRequestModelBuilder().Build(), user.Id);
    }

    public static async Task<PurposeModel> PurposeCreate(IPurposeEndpoint endpoint, WorkspaceModel workspace, UserModel user)
    {
        return await endpoint.PurposeCreate(new PurposeCreateRequestModelBuilder(workspace.Id).Build(), user.Id);
    }
}
