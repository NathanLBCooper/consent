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
        var request = new UserCreateRequestModelBuilder().Build();

        void Verify(UserModel model)
        {
            model.Name.ShouldBe(request.Name);
            model.WorkspaceMemberships.ShouldBeEmpty();
        }

        var created = await _sut.UserCreate(request);
        Verify(created);

        var fetched = await _sut.UserGet(created.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);

        fetched = await _sut.UserGet(created.Id, created.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Can_get_workspace_memberships_for_user()
    {
        var created = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());
        var workspace = await _workspaceEndpoint.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), created.Id);

        void Verify(UserModel model)
        {
            var membership = model.WorkspaceMemberships.ShouldHaveSingleItem();
            membership.Workspace.Id.ShouldBe(workspace.Id);
            membership.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
            membership.Permissions.ShouldBeEquivalentTo(
                new[] {
                WorkspacePermissionModel.View, WorkspacePermissionModel.Edit,
                WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
                );
        }

        var fetched = await _sut.UserGet(created.Id);
        Verify(fetched);

        fetched = await _sut.UserGet(created.Id, created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Cannot_get_nonexistant_user()
    {
        var user = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());

        var fetch = async () => await _sut.UserGet(-1, user.Id);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_user_with_nonexistant_requesting_user()
    {
        var user = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());

        var fetch = async () => await _sut.UserGet(user.Id, -1);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task User_can_only_get_themselves() // ie pointless endpoint. But I would like user permissions or something soon
    {
        var bob = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());
        var eve = await _sut.UserCreate(new UserCreateRequestModelBuilder().Build());

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
