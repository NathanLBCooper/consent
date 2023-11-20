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
using static Consent.Tests.Builders.EndpointExtensions;

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
        var user = await UserCreate(_userEndpoint);
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var createdWorkspace = await _sut.WorkspaceCreate(request, user.Id);
        Verify(createdWorkspace);

        var fetchedWorkspace = await _sut.WorkspaceGet(createdWorkspace.Id, user.Id);
        fetchedWorkspace.Id.ShouldBe(createdWorkspace.Id);
        Verify(fetchedWorkspace);

        void Verify(WorkspaceModel w)
        {
            w.Name.ShouldBe(request.Name);
            w.Memberships.ShouldNotBeEmpty();
        }
    }

    [Fact]
    public async Task Cannot_get_nonexistant_workspace()
    {
        var user = await UserCreate(_userEndpoint);
        var workspaceId = -1;

        var fetch = async () => await _sut.WorkspaceGet(workspaceId, user.Id);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_workspace_with_nonexistant_user()
    {
        var workspaceCreator = await UserCreate(_userEndpoint);
        var workspace = await WorkspaceCreate(_sut, workspaceCreator);
        var userId = -1;

        var eveFetchesAlicesWorkspace = async () => await _sut.WorkspaceGet(workspace.Id, userId);

        var error = await eveFetchesAlicesWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_workspace_with_user_with_no_permissions()
    {
        var alice = await UserCreate(_userEndpoint);
        var alicesWorkspace = await WorkspaceCreate(_sut, alice);
        var eve = await UserCreate(_userEndpoint);

        var eveFetchesAlicesWorkspace = async () => await _sut.WorkspaceGet(alicesWorkspace.Id, eve.Id);

        var error = await eveFetchesAlicesWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_create_workspace_with_nonexistant_user()
    {
        var userId = -1;

        var createWorkspace = async () => await _sut.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), userId);

        var error = await createWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var creator = await UserCreate(_userEndpoint);
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var createdWorkspace = await _sut.WorkspaceCreate(request, creator.Id);

        var membership = createdWorkspace.Memberships.ShouldHaveSingleItem();
        membership.UserId.ShouldBe(creator.Id);
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
