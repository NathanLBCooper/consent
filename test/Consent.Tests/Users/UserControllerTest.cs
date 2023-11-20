using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain.Core;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;
using static Consent.Tests.Builders.EndpointExtensions;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IUserEndpoint _sut;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(
            new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var createRequest = new UserCreateRequestModelBuilder().Build();

        var createdUser = await _sut.UserCreate(createRequest);
        Verify(createdUser);

        var selfFetchedUser = await _sut.UserGet(createdUser.Id);
        selfFetchedUser.Id.ShouldBe(createdUser.Id);
        Verify(selfFetchedUser);

        var fetchedUser = await _sut.UserGet(createdUser.Id, createdUser.Id);
        fetchedUser.Id.ShouldBe(createdUser.Id);
        Verify(fetchedUser);

        void Verify(UserModel u)
        {
            u.Name.ShouldBe(createRequest.Name);
            u.WorkspaceMemberships.ShouldBeEmpty();
        }
    }

    [Fact]
    public async Task Can_get_workspace_memberships_for_user()
    {
        var createdUser = await UserCreate(_sut);
        var workspace = await WorkspaceCreate(_workspaceEndpoint, createdUser);

        var selfFetchedUser= await _sut.UserGet(createdUser.Id);
        Verify(selfFetchedUser);

        var fetchedUser = await _sut.UserGet(createdUser.Id, createdUser.Id);
        Verify(fetchedUser);

        void Verify(UserModel u)
        {
            var membership = u.WorkspaceMemberships.ShouldHaveSingleItem();
            membership.Workspace.Id.ShouldBe(workspace.Id);
            membership.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
            membership.Permissions.ShouldBeEquivalentTo(
                new[] {
                WorkspacePermissionModel.View, WorkspacePermissionModel.Edit,
                WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
                );
        }
    }

    [Fact]
    public async Task Cannot_get_nonexistant_user()
    {
        var userId = -1;
        var requestingUser = await UserCreate(_sut);

        var fetch = async () => await _sut.UserGet(userId, requestingUser.Id);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_user_with_nonexistant_requesting_user()
    {
        var user = await UserCreate(_sut);
        var requestingUserId = -1;

        var fetch = async () => await _sut.UserGet(user.Id, requestingUserId);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task User_can_only_get_themselves() // ie pointless endpoint. But I would like user permissions or something soon
    {
        var bob = await UserCreate(_sut);
        var eve = await UserCreate(_sut);

        var bobFetchedByBob = await _sut.UserGet(bob.Id, bob.Id);
        bobFetchedByBob.Id.ShouldBe(bob.Id);

        var bobFetchedByEve = async () => await _sut.UserGet(bob.Id, eve.Id);

        var error = await bobFetchedByEve.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
