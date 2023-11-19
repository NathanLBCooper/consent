using System;
using System.Net;
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
public class ContractControllerTest_Contract : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest_Contract(DatabaseFixture fixture)
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
        var user = await CreateUser();
        var workspace = await CreateWorkspace(user);
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

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_non_existant_contract()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_with_user_without_view_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact]
    public async Task Cannot_create_a_contract_on_a_nonexistant_workspace()
    {
        var user = await CreateUser();
        var request = new ContractCreateRequestModelBuilder(-1).Build();

        var create = async () => await _sut.ContractCreate(request, user.Id);

        var error = await create.ShouldThrowAsync<ApiException>();
        ((int)error.StatusCode).ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_with_user_without_edit_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
        // todo maybe it should be unauthorized, but if no view permissions as well, notfound
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

    public void Dispose()
    {
        _client.Dispose();
    }
}
