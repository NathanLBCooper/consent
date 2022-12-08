using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consent.Domain;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.StorageContext;
using Shouldly;

namespace Consent.Tests.Workspaces;

[Collection("DatabaseTest")]
public class WorkspaceEndpointTest
{
    private readonly WorkspaceEndpoint _sut;
    private readonly UserEndpoint _userEndpoint;

    public WorkspaceEndpointTest(DatabaseFixture fixture)
    {
        var unitOfWorkContext = fixture.CreateUnitOfWorkContext();

        var workspaceRepository = new WorkspaceRepository(unitOfWorkContext);
        var userRepository = new UserRepository(unitOfWorkContext);
        _sut = new WorkspaceEndpoint(workspaceRepository, userRepository, unitOfWorkContext);
        _userEndpoint = new UserEndpoint(userRepository, unitOfWorkContext);
    }

    [Fact]
    public async Task Can_create_and_get_a_workspace()
    {
        var ctx = await CreateUserContext();

        var workspace = NewWorkspace();
        var created = await _sut.WorkspaceCreate(workspace, ctx);

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(workspace.Name);

        var fetched = await _sut.WorkspaceGet(created.Id, ctx);

        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    [Fact]
    public void Cannot_create_workspace_with_nonexistant_user()
    {
        var workspace = NewWorkspace();
        var action = async () => await _sut.WorkspaceCreate(workspace, new Context { UserId = new UserId(-1) });

        _ = action.ShouldThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Workspace_creater_gets_all_permissions()
    {
        var ctx = await CreateUserContext();

        var created = await _sut.WorkspaceCreate(NewWorkspace(), ctx);

        var permissions = await _sut.WorkspacePermissionsGet(created.Id, ctx);
        permissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermission.View, WorkspacePermission.Edit, WorkspacePermission.Admin, WorkspacePermission.Buyer }
            );
    }

    [Fact]
    public async Task Permissions_are_empty_if_there_shouldnt_be_permissions()
    {
        var ctx = await CreateUserContext();

        var created = await _sut.WorkspaceCreate(NewWorkspace(), ctx);

        var permissionsForNonexistant = await _sut.WorkspacePermissionsGet(new WorkspaceId(-1), ctx);
        permissionsForNonexistant.ShouldBeEmpty();

        var anotherUserCtx = await CreateUserContext();
        var permissionsForAnotherUser = await _sut.WorkspacePermissionsGet(created.Id, anotherUserCtx);
        permissionsForAnotherUser.ShouldBeEmpty();
    }

    [Fact]
    public async Task Can_get_workspace_memberships()
    {
        var ctx = await CreateUserContext();

        var created = await _sut.WorkspaceCreate(NewWorkspace(), ctx);
        var memberships = await _sut.WorkspaceMembersGet(created.Id, ctx);

        _ = memberships.ShouldNotBeNull();

        var membership = memberships[0];
        membership.UserId.ShouldBe(ctx.UserId);
        membership.WorkspaceId.ShouldBe(created.Id);
        membership.Permissions.ShouldBeEquivalentTo(
            new[] { WorkspacePermission.View, WorkspacePermission.Edit, WorkspacePermission.Admin, WorkspacePermission.Buyer }
            );

        // todo add more user to workspace
    }

    // to check only admin can Can_get_workspace_memberships

    private async Task<Context> CreateUserContext([CallerMemberName] string callerName = "")
    {
        return new Context { UserId = (await _userEndpoint.UserCreate(new User($"{callerName}-user"))).Id };
    }

    private Workspace NewWorkspace([CallerMemberName] string callerName = "")
    {
        return new Workspace($"{callerName}-workspace");
    }
}
