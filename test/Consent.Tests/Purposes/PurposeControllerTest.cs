using System;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Purposes;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;

namespace Consent.Tests.Purposes;

[Collection("DatabaseTest")]
public class PurposeControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IPurposeEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public PurposeControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(
            new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IPurposeEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact(Skip = "unimplemented")]
    public async Task Can_create_and_get_a_permission()
    {
        var user = await CreateUser();
        var workspace = await CreateWorkspace(user);
        var request = new PurposeCreateRequestModelBuilder(workspace.Id).Build();

        void Verify(PurposeModel model)
        {
            model.Name.ShouldBe(request.Name);
            model.Description.ShouldBe(request.Name);
            model.Workspace.Id.ShouldBe(workspace.Id);
            model.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
        }

        var created = await _sut.PurposeCreate(request, user.Id);
        Verify(created);

        var fetched = await _sut.PurposeGet(created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    // Cannot_get_nonexistant_purpose
    // Cannot_get_purpose_with_nonexistant_user
    // Cannot_get_purpose_with_user_with_no_permission
    // ...

    // Cannot_create_purpose_with_nonexistant_user
    // Cannot_create_purpose_with_nonexistant_workspace
    // Cannot_create_purpose_with_user_with_no_permission
    // ...

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
