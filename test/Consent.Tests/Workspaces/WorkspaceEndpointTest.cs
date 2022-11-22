using Consent.Domain;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.StorageContext;
using Shouldly;
using System.Threading.Tasks;

namespace Consent.Tests.Workspaces
{
    [Collection("DatabaseTest")]
    public class WorkspaceEndpointTest
    {
        private readonly DatabaseFixture _fixture;
        private readonly WorkspaceEndpoint _sut;
        private readonly UserEndpoint _userEndpoint;

        public WorkspaceEndpointTest(DatabaseFixture fixture)
        {
            _fixture = fixture;

            var workspaceRepository = new WorkspaceRepository(_fixture.GetConnection);
            _sut = new WorkspaceEndpoint(workspaceRepository, _fixture.CreateUnitOfWork);

            var userRepository = new UserRepository(_fixture.GetConnection);
            _userEndpoint = new UserEndpoint(userRepository, _fixture.CreateUnitOfWork);
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

            var permissionsForNonexistant = await _sut.WorkspacePermissionsGet(-1, ctx);
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
            memberships[0].UserId.ShouldBe(ctx.UserId);
            // todo etc


        }

        // to check only admin can Can_get_workspace_memberships

        private async Task<Context> CreateUserContext() => new Context { UserId = (await _userEndpoint.UserCreate(new User("somename"))).Id };
    }
}
