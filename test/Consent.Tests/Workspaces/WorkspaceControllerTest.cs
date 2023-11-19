using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain.Core;
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
        var factory = new TestWebApplicationFactory(
            new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IWorkspaceEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_workspace()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        void Verify(WorkspaceModel model)
        {
            model.Name.ShouldBe(request.Name);
            model.Memberships.ShouldNotBeEmpty();
        }

        var created = await _sut.WorkspaceCreate(request, user.Id);
        Verify(created);

        var fetched = await _sut.WorkspaceGet(created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Cannot_get_nonexistant_workspace()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());

        var fetch = async () => await _sut.WorkspaceGet(-1, user.Id);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_workspace_with_nonexistant_user()
    {
        var userBuilder = new UserCreateRequestModelBuilder();
        var user = await _userEndpoint.UserCreate(userBuilder.Build());
        var workspace = await _sut.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), user.Id);

        var eveFetchesAlicesWorkspace = async () => await _sut.WorkspaceGet(workspace.Id, -1);

        var error = await eveFetchesAlicesWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_workspace_with_user_with_no_permissions()
    {
        var userBuilder = new UserCreateRequestModelBuilder();
        var alice = await _userEndpoint.UserCreate(userBuilder.Build());
        var alicesWorkspace = await _sut.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), alice.Id);
        var eve = await _userEndpoint.UserCreate(userBuilder.Build());

        var eveFetchesAlicesWorkspace = async () => await _sut.WorkspaceGet(alicesWorkspace.Id, eve.Id);

        var error = await eveFetchesAlicesWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_create_workspace_with_nonexistant_user()
    {
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var createWorkspace = async () => await _sut.WorkspaceCreate(request, -1);

        var error = await createWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var user = await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var created = await _sut.WorkspaceCreate(request, user.Id);

        var membership = created.Memberships.ShouldHaveSingleItem();
        membership.UserId.ShouldBe(user.Id);
        membership.Permissions.ShouldBeEquivalentTo(
            new[] {
                WorkspacePermissionModel.View, WorkspacePermissionModel.Edit,
                WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
