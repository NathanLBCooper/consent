using System;
using System.Linq;
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

    [Fact]
    public async Task Cannot_create_a_contract_on_a_nonexistant_workspace()
    {
        var user = await CreateUser();
        var request = new ContractCreateRequestModelBuilder(-1).Build();

        var create = async () => await _sut.ContractCreate(request, user.Id);

        var error = await create.ShouldThrowAsync<ApiException>();
        ((int)error.StatusCode).ShouldBe((int)HttpStatusCode.NotFound);
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

    [Fact]
    public async Task Can_create_a_provision()
    {
        var user = await CreateUser();
        var contract = await CreateContact(await CreateWorkspace(user), user);
        var version = await CreateVersion(contract, user);
        var permissionId = 1; // todo, doesn't validate existance
        var request = new ProvisionCreateRequestModelBuilder(new[] { permissionId }).Build();

        void Verify(ProvisionModel model)
        {
            model.Text.ShouldBe(request.Text);
            var permission = model.Permissions.ShouldHaveSingleItem();
            permission.Id.ShouldBe(permissionId);
            permission.Href.ShouldBe(null); // todo, no controller
            model.Version.Id.ShouldBe(version.Id);
            model.Version.Href.ShouldBe($"/Contract/{contract.Id}/version/{version.Id}");
        }

        var created = await _sut.ProvisionCreate(contract.Id, version.Id, request, user.Id);
        Verify(created);

        var fetchedVersion = await _sut.ContractVersionGet(contract.Id, created.Id, user.Id);
        var fetched = fetchedVersion.Provisions.Single(p => p.Id == created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Cannot_create_a_provision_without_any_permissions()
    {
        var user = await CreateUser();
        var contract = await CreateContact(await CreateWorkspace(user), user);
        var version = await CreateVersion(contract, user);
        var request = new ProvisionCreateRequestModelBuilder(Array.Empty<int>()).Build();

        var createProvision = async () => await _sut.ProvisionCreate(contract.Id, version.Id, request, user.Id);

        var error = await createProvision.ShouldThrowAsync<ApiException>();
        ((int)error.StatusCode).ShouldBe((int)HttpStatusCode.UnprocessableEntity);
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

    private async Task<ContractVersionModel> CreateVersion(ContractModel contract, UserModel user)
    {
        return await _sut.ContractVersionCreate(
            contract.Id, new ContractVersionCreateRequestModelBuilder().Build(), user.Id);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
