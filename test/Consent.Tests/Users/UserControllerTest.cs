using System;
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

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IUserEndpoint _sut;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var request = new UserCreateRequestModelBuilder().Build();

        var created = await _sut.UserCreate(request);
        created.Name.ShouldBe(request.Name);

        var fetched = await _sut.UserGet(created.Id);
        _ = fetched.ShouldNotBeNull();

        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    [Fact]
    public async Task Can_get_workspace_memberships_for_user()
    {
        var created = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());
        var workspace = await _workspaceEndpoint.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), created.Id);

        var fetched = await _sut.UserGet(created.Id);

        var membership = fetched.WorkspaceMemberships.ShouldHaveSingleItem();
        membership.Workspace.Id.ShouldBe(workspace.Id);
        membership.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
        membership.Permissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    [Fact]
    public async Task Etags_work()
    {
        var request = new UserCreateRequestModelBuilder().Build();

        var created = await _sut.UserCreateReq(request);
        var etag = created.Headers.ETag?.ToString();
        _ = etag.ShouldNotBeNull();
        var user = Guard.NotNull(created.Content);

        var getWithEtag = await _sut.UserGetReq(user.Id, etag);
        getWithEtag.StatusCode.ShouldBe(HttpStatusCode.NotModified);
        getWithEtag.Content.ShouldBeNull();

        var getWithoutEtag = await _sut.UserGetReq(user.Id, null);
        getWithoutEtag.StatusCode.ShouldBe(HttpStatusCode.OK);
        _ = getWithoutEtag.Content.ShouldNotBeNull();

        var getWithWrongEtag = await _sut.UserGetReq(user.Id, "wrong etag");
        getWithWrongEtag.StatusCode.ShouldBe(HttpStatusCode.OK);
        _ = getWithWrongEtag.Content.ShouldNotBeNull();

        var otherUser = await _sut.UserCreate(request);
        var getWithOtherUser = await _sut.UserGetReq(otherUser.Id, etag);
        getWithOtherUser.StatusCode.ShouldBe(HttpStatusCode.OK);
        _ = getWithOtherUser.Content.ShouldNotBeNull();

        // todo test that update invalidates etag
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
