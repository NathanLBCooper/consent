using System.Linq;
using System.Threading.Tasks;
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
using Shouldly;

namespace Consent.Tests.Workspaces;

[Collection("DatabaseTest")]
public class WorkspaceControllerTest
{
    private readonly WorkspaceController _sut;
    private readonly UserControllerTestWrapper _userController;

    public WorkspaceControllerTest(DatabaseFixture fixture)
    {
        var workspaceRepository = new WorkspaceRepository(fixture.WorkspaceDbContext);
        var userRepository = new UserRepository(fixture.UserDbContext);
        _sut = new WorkspaceController(new NullLogger<WorkspaceController>(), workspaceRepository, userRepository);
        _userController = new UserControllerTestWrapper(new UserController(new NullLogger<UserController>(), new FakeLinkGenerator(), userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        });
    }

    [Fact]
    public async Task Can_create_and_get_a_workspace()
    {
        var user = Guard.NotNull((await _userController.UserCreate(new UserCreateRequestModelBuilder().Build())).GetValue());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var created = (await _sut.WorkspaceCreate(request, user.Id)).GetValue();

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);
        created.Memberships.ShouldNotBeEmpty();

        var fetched = (await _sut.WorkspaceGet(created.Id, user.Id)).GetValue();

        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
        fetched.Memberships.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Cannot_create_workspace_with_nonexistant_user()
    {
        var request = new WorkspaceCreateRequestModelBuilder().Build();
        var response = await _sut.WorkspaceCreate(request, -1);

        _ = response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Cannot_get_workspace_with_user_with_no_permissions()
    {
        var userBuilder = new UserCreateRequestModelBuilder();
        var user = Guard.NotNull((await _userController.UserCreate(userBuilder.Build())).GetValue());
        var otherUser = Guard.NotNull((await _userController.UserCreate(userBuilder.Build())).GetValue());
        var created = Guard.NotNull((await _sut.WorkspaceCreate(new WorkspaceCreateRequestModelBuilder().Build(), user.Id)).GetValue());

        var response = await _sut.WorkspaceGet(created.Id, otherUser.Id);

        _ = response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var user = Guard.NotNull((await _userController.UserCreate(new UserCreateRequestModelBuilder().Build())).GetValue());
        var request = new WorkspaceCreateRequestModelBuilder().Build();

        var created = Guard.NotNull((await _sut.WorkspaceCreate(request, user.Id)).GetValue());
        var userPermissions = created.Memberships.Single(m => m.UserId == user.Id).Permissions;

        userPermissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermissionModel.View, WorkspacePermissionModel.Edit, WorkspacePermissionModel.Admin, WorkspacePermissionModel.Buyer }
            );
    }
}
