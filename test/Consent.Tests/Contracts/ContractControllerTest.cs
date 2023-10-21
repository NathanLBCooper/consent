using System;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Contracts;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;

namespace Consent.Tests.Contracts;

[Collection("DatabaseTest")]
public class ContractControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IContractEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_contract()
    {
        var (user, workspace) = await CreateUserAndWorkspace();
        var request = new ContractCreateRequestModelBuilder(workspace.Id).Build();

        void Verify(ContractModel model)
        {
            model.Name.ShouldBe(request.Name);
            model.Workspace.Id.ShouldBe(workspace.Id);
            model.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
            model.Versions.ShouldBeEmpty();
        }

        var created = await _sut.ContractCreate(request, user.Id);
        Verify(created);

        var fetched = await _sut.ContractGet(created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Can_create_and_get_a_contract_version()
    {
        var (user, _, contract) = await CreateUserWorkspaceAndContract();
        var request = new ContractVersionCreateRequestModelBuilder().Build();

        void Verify(ContractVersionModel model)
        {
            model.Contract.Id.ShouldBe(contract.Id);
            model.Contract.Href.ShouldBe($"/Contract/{contract.Id}");
            model.Name.ShouldBe(model.Name);
            model.Text.ShouldBe(model.Text);
            model.Status.ShouldBe(ContractVersionStatusModel.Draft);
        }

        var created = await _sut.ContractVersionCreate(contract.Id, request, user.Id);
        Verify(created);

        var fetched = await _sut.ContractVersionGet(contract.Id, created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    private async Task<(UserModel user, WorkspaceModel workspace)> CreateUserAndWorkspace()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());

        var workspace = await _workspaceEndpoint.WorkspaceCreate(
            request: new WorkspaceCreateRequestModelBuilder().Build(),
            userId: user.Id
            );

        return (user, workspace);
    }

    private async Task<(UserModel user, WorkspaceModel workspace, ContractModel contract)> CreateUserWorkspaceAndContract()
    {
        var (user, workspace) = await CreateUserAndWorkspace();
        var contract = await _sut.ContractCreate(new ContractCreateRequestModelBuilder(workspace.Id).Build(), user.Id);

        return (user, workspace, contract);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
