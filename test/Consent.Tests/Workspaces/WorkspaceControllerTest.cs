using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;

namespace Consent.Tests.Workspaces;

[Collection("DatabaseTest")]
public class WorkspaceControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IWorkspaceEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;

    public WorkspaceControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IWorkspaceEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_workspace()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var created = await _sut.WorkspaceCreate(request, user.Id);

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);
        created.Memberships.ShouldNotBeEmpty();

        var fetched = await _sut.WorkspaceGet(created.Id, user.Id);

        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
        fetched.Memberships.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Cannot_create_workspace_with_nonexistant_userAsync()
    {
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var createWorkspace = async () => await _sut.WorkspaceCreate(request, -1);

        var ex = await createWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(ex.Content).Status.ShouldBe((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Cannot_get_workspace_with_user_with_no_permissions()
    {
        var userBuilder = new UserCreateRequestModelBuilder();
        var userOne = await _userEndpoint.UserCreate(userBuilder.Build());
        var userTwo = await _userEndpoint.UserCreate(userBuilder.Build());
        var userOnesWorkspace = await _sut.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), userOne.Id);

        var fetchAnotherUsersWorkspace = async () => await _sut.WorkspaceGet(userOnesWorkspace.Id, userTwo.Id);

        var ex = await fetchAnotherUsersWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(ex.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var created = await _sut.WorkspaceCreate(request, user.Id);
        var userPermissions = created.Memberships.Single(m => m.UserId == user.Id).Permissions;

        userPermissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
