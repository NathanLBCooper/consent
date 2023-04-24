using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consent.Api.Controllers;
using Consent.Api.Models;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Shouldly;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest
{
    private readonly UserController _sut;
    private readonly WorkspaceController _workspaceController;
    private readonly HeaderTestHelper _headerTestHelper;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var userRepository = new UserRepository(fixture.UserDbContext);
        _sut = new UserController(new NullLogger<UserController>(), new FakeLinkGenerator(), userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };

        var workspaceRepository = new WorkspaceRepository(fixture.WorkspaceDbContext);
        _workspaceController = new WorkspaceController(
            new NullLogger<WorkspaceController>(), workspaceRepository, userRepository
            );

        _headerTestHelper = new HeaderTestHelper(_sut);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var request = UserRequest();
        var created = (await CreateUser(request)).GetValue();
        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);

        var fetched = (await GetUser(created.Id)).GetValue();
        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    [Fact]
    public async Task Can_get_workspace_memberships_for_user()
    {
        var request = UserRequest();
        var created = (await CreateUser(request)).GetValue();
        _ = created.ShouldNotBeNull();
        var workspace = (await _workspaceController.WorkspaceCreate(WorkspaceRequest(), created.Id)).GetValue<WorkspaceModel>();
        _ = workspace.ShouldNotBeNull();

        var fetched = (await GetUser(created.Id)).GetValue();
        _ = fetched.ShouldNotBeNull();

        var membership = fetched.WorkspaceMemberships.ShouldHaveSingleItem();
        membership.Workspace.Id.ShouldBe(workspace.Id);
        membership.Workspace.Href.ShouldBe("FakeLinkGenerator:Id:1,action:WorkspaceGet,controller:Workspace");
        membership.Permissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    [Fact]
    public async Task Etags_work()
    {
        var request = UserRequest();
        var created = (await CreateUser(request)).GetValue();
        _ = created.ShouldNotBeNull();
        var etag = _headerTestHelper.GetRecordedHeader(HeaderNames.ETag);

        var fetched = (await GetUser(created.Id)).GetValue();
        _ = fetched.ShouldNotBeNull();
        _headerTestHelper.GetRecordedHeader(HeaderNames.ETag).ShouldBe(etag);

        var result = (await GetUser(created.Id, etag)).Result as StatusCodeResult;
        _ = result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(304);

        _ = (await GetUser(created.Id, "this is not the etag")).Result.ShouldBeOfType<OkObjectResult>();

        var otherUser = (await CreateUser(request)).GetValue();
        _ = otherUser.ShouldNotBeNull();
        _ = (await GetUser(otherUser.Id, etag)).Result.ShouldBeOfType<OkObjectResult>();
    }

    private UserCreateRequestModel UserRequest([CallerMemberName] string callerName = "")
    {
        return new UserCreateRequestModel { Name = $"{callerName}-workspace" };
    }

    private async Task<ActionResult<UserModel>> CreateUser(UserCreateRequestModel request)
    {
        var response = await _sut.UserCreate(request);
        _headerTestHelper.RecordLastHeaders();
        _headerTestHelper.ClearHeaders();
        return response;
    }

    private async Task<ActionResult<UserModel>> GetUser(int userId, string? ifNoneMatch = null)
    {
        var response = await _sut.UserGet(userId, ifNoneMatch);
        _headerTestHelper.RecordLastHeaders();
        _headerTestHelper.ClearHeaders();
        return response;
    }

    private WorkspaceCreateRequestModel WorkspaceRequest([CallerMemberName] string callerName = "")
    {
        return new WorkspaceCreateRequestModel { Name = $"{nameof(UserControllerTest)}-{callerName}-workspace" };
    }
}
