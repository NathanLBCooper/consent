using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consent.Api.Controllers;
using Consent.Api.Models;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Consent.Tests.Workspaces;

[Collection("DatabaseTest")]
public class WorkspaceControllerTest
{
    private readonly WorkspaceController _sut;
    private readonly UserController _userController;

    public WorkspaceControllerTest(DatabaseFixture fixture)
    {
        var workspaceRepository = new WorkspaceRepository(fixture.WorkspaceDbContext);
        var userRepository = new UserRepository(fixture.UserDbContext);
        _sut = new WorkspaceController(new NullLogger<WorkspaceController>(), workspaceRepository, userRepository);
        _userController = new UserController(new NullLogger<UserController>(), userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    [Fact]
    public async Task Can_create_and_get_a_workspace()
    {
        var user = await CreateUser();
        var request = WorkspaceRequest();

        var created = (await _sut.WorkspaceCreate(request, user.Id)).GetValue<WorkspaceModel>();

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);
        created.Memberships.ShouldNotBeEmpty();

        var fetched = (await _sut.WorkspaceGet(created.Id, user.Id)).GetValue<WorkspaceModel>();

        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
        fetched.Memberships.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Cannot_create_workspace_with_nonexistant_user()
    {
        var request = WorkspaceRequest();
        var response = await _sut.WorkspaceCreate(request, -1);

        _ = response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Cannot_get_workspace_with_user_with_no_permissions()
    {
        var user = await CreateUser();
        var otherUser = await CreateUser();
        var created = (await _sut.WorkspaceCreate(WorkspaceRequest(), user.Id)).GetValue<WorkspaceModel>();
        _ = created.ShouldNotBeNull();

        var response = await _sut.WorkspaceGet(created.Id, otherUser.Id);

        _ = response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var user = await CreateUser();
        var request = WorkspaceRequest();

        var created = (await _sut.WorkspaceCreate(request, user.Id)).GetValue<WorkspaceModel>();
        _ = created.ShouldNotBeNull();
        var userPermissions = created.Memberships.Single(m => m.UserId == user.Id).Permissions;

        userPermissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }

    private async Task<UserModel> CreateUser([CallerMemberName] string callerName = "")
    {
        var request = new UserCreateRequestModel { Name = $"{callerName}-workspace" };
        var user = (await _userController.UserCreate(request)).GetValue();
        _ = user.ShouldNotBeNull();

        _userController.HttpContext.Request.Headers.Clear();
        _userController.HttpContext.Response.Headers.Clear();

        return user;
    }

    private WorkspaceCreateRequestModel WorkspaceRequest([CallerMemberName] string callerName = "")
    {
        return new WorkspaceCreateRequestModel { Name = $"{nameof(WorkspaceControllerTest)}-{callerName}-workspace" };
    }
}
