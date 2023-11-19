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
public class ContractControllerTest_Version : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest_Version(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IContractEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_contract_version()
    {
        var user = await CreateUser();
        var contract = await CreateContact(await CreateWorkspace(user), user);
        var request = new ContractVersionCreateRequestModelBuilder().Build();

        void Verify(ContractVersionModel model)
        {
            model.Contract.Id.ShouldBe(contract.Id);
            model.Contract.Href.ShouldBe($"/Contract/{contract.Id}");
            model.Name.ShouldBe(model.Name);
            model.Text.ShouldBe(model.Text);
            model.Status.ShouldBe(ContractVersionStatusModel.Draft);
            model.Provisions.ShouldBeEmpty();
        }

        var created = await _sut.ContractVersionCreate(contract.Id, request, user.Id);
        Verify(created);

        var fetched = await _sut.ContractVersionGet(contract.Id, created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_on_a_non_existant_contract()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_with_user_without_edit_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
    }

    private async Task<UserModel> CreateUser()
    {
        return await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
    }

    private async Task<WorkspaceModel> CreateWorkspace(UserModel user)
    {
        return await _workspaceEndpoint.WorkspaceCreate(
            request: new WorkspaceCreateRequestModelBuilder().Build(),
            userId: user.Id
            );
    }

    private async Task<ContractModel> CreateContact(WorkspaceModel workspace, UserModel user)
    {
        return await _sut.ContractCreate(new ContractCreateRequestModelBuilder(workspace.Id).Build(), user.Id);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
