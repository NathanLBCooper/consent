using Consent.Domain;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.StorageContext;
using Shouldly;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Consent.Tests.Workspaces
{
    [Collection("DatabaseTest")]
    public class WorkspaceEndpointTest
    {
        private readonly WorkspaceEndpoint _sut;
        private readonly UserEndpoint _userEndpoint;

        public WorkspaceEndpointTest(DatabaseFixture fixture)
        {
            var workspaceRepository = new WorkspaceRepository(fixture.GetConnection);
            _sut = new WorkspaceEndpoint(workspaceRepository, fixture.CreateUnitOfWork);

            var userRepository = new UserRepository(fixture.GetConnection);
            _userEndpoint = new UserEndpoint(userRepository, fixture.CreateUnitOfWork);
        }

        [Fact]
        public async Task Can_create_and_get_a_workspace()
        {
            var ctx = await CreateUserContext();

            var workspace = new Workspace("someworkspacename");
            var created = await _sut.WorkspaceCreate(workspace, ctx);

            created.ShouldNotBeNull();
            created.Name.ShouldBe(workspace.Name);

            var fetched = await _sut.WorkspaceGet(created.Id, ctx);

            fetched.ShouldNotBeNull();
            fetched.Id.ShouldBe(created.Id);
            fetched.Name.ShouldBe(created.Name);
        }

        [Fact]
        public async Task Workspace_creater_gets_all_permissions()
        {
            var ctx = await CreateUserContext();

            var created = await _sut.WorkspaceCreate(new Workspace("someworkspacename"), ctx);

            var permissions = await _sut.WorkspacePermissionsGet(created.Id, ctx);
            permissions.ShouldBeEquivalentTo(
                new[] { WorkspacePermission.View, WorkspacePermission.Edit, WorkspacePermission.Admin, WorkspacePermission.Buyer }
                );
        }

        [Fact]
        public async Task Permissions_are_empty_if_there_shouldnt_be_permissions()
        {
            var ctx = await CreateUserContext();

            var created = await _sut.WorkspaceCreate(new Workspace("someworkspacename"), ctx);

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

            var created = await _sut.WorkspaceCreate(new Workspace("someworkspacename"), ctx);
            var memberships = await _sut.WorkspaceMembersGet(created.Id, ctx);

            memberships.ShouldNotBeNull();

            var membership = memberships[0];
            membership.UserId.ShouldBe(ctx.UserId);
            membership.WorkspaceId.ShouldBe(created.Id);
            membership.Permissions.ShouldBeEquivalentTo(
                new[] { WorkspacePermission.View, WorkspacePermission.Edit, WorkspacePermission.Admin, WorkspacePermission.Buyer }
                );

            // todo add more user to workspace
        }

        // to check only admin can Can_get_workspace_memberships

        private async Task<Context> CreateUserContext([CallerMemberName] string callerName = "") =>
            new Context { UserId = (await _userEndpoint.UserCreate(new User(callerName))).Id };
    }
}
