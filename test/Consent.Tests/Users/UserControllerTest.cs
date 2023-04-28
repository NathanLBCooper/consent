using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Api.Users;
using Consent.Api.Workspaces;
using Consent.Domain;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Shouldly;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest
{
    private readonly UserControllerTestWrapper _sut;
    private readonly WorkspaceController _workspaceController;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var userRepository = new UserRepository(fixture.UserDbContext);
        var sut = new UserController(new NullLogger<UserController>(), new FakeLinkGenerator(), userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
        _sut = new UserControllerTestWrapper(sut);

        var workspaceRepository = new WorkspaceRepository(fixture.WorkspaceDbContext);
        _workspaceController = new WorkspaceController(
            new NullLogger<WorkspaceController>(), workspaceRepository, userRepository
            );
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var request = new UserCreateRequestModelBuilder().Build();

        var created = await CreateUser(request);
        created.Name.ShouldBe(request.Name);

        var fetched = (await _sut.UserGet(created.Id, null)).GetValue();
        _ = fetched.ShouldNotBeNull();

        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    [Fact]
    public async Task Can_get_workspace_memberships_for_user()
    {
        var created = Guard.NotNull(await CreateUser());
        var workspace = Guard.NotNull((await _workspaceController.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), created.Id)).GetValue());

        var fetched = Guard.NotNull((await _sut.UserGet(created.Id, null)).GetValue());

        var membership = fetched.WorkspaceMemberships.ShouldHaveSingleItem();
        membership.Workspace.Id.ShouldBe(workspace.Id);
        membership.Workspace.Href.ShouldBe($"FakeLinkGenerator:Id:{workspace.Id},action:WorkspaceGet,controller:Workspace");
        membership.Permissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    [Fact]
    public async Task Etags_work()
    {
        var request = new UserCreateRequestModelBuilder().Build();
        var created = Guard.NotNull((await _sut.UserCreate(request)).GetValue());
        var etag = _sut.LastResponseHeaders[HeaderNames.ETag];

        var getWithEtag = await _sut.UserGet(created.Id, ifNoneMatch: etag);
        var result = getWithEtag.Result as StatusCodeResult;
        _ = result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(304);
        _sut.LastResponseHeaders[HeaderNames.ETag].ShouldBe(etag);

        _ = (await _sut.UserGet(created.Id, ifNoneMatch: null)).Result.ShouldBeOfType<OkObjectResult>();
        _sut.LastResponseHeaders[HeaderNames.ETag].ShouldBe(etag);
        _ = (await _sut.UserGet(created.Id, "this is not the etag")).Result.ShouldBeOfType<OkObjectResult>();
        _sut.LastResponseHeaders[HeaderNames.ETag].ShouldBe(etag);

        var otherUser = Guard.NotNull((await _sut.UserCreate(request)).GetValue());
        _ = (await _sut.UserGet(otherUser.Id, etag)).Result.ShouldBeOfType<OkObjectResult>();
        _sut.LastResponseHeaders[HeaderNames.ETag].ShouldNotBe(etag);
    }

    private async Task<UserModel> CreateUser(UserCreateRequestModel? request = null)
    {
        request ??= new UserCreateRequestModelBuilder().Build();
        var user = Guard.NotNull((await _sut.UserCreate(request)).GetValue());

        return user;
    }
}
