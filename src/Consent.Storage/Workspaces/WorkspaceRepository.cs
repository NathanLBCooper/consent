using Consent.Domain.Workspaces;
using Consent.Storage.UnitOfWork;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace Consent.Storage.Workspaces
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IGetConnection _getConnection;

        public WorkspaceRepository(IGetConnection getConnection)
        {
            _getConnection = getConnection;
        }

        public async Task<WorkspaceEntity?> Get(int id)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
select * from [dbo].[Workspace] where [Id] = @id;
";

            return await connection.QuerySingleOrDefaultAsync<WorkspaceEntity?>(query, new { id }, transaction);
        }

        public async Task<WorkspaceEntity> Create(Workspace workspace)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
insert into [dbo].[Workspace] ([Name]) values (@name);
select SCOPE_IDENTITY();
";
            var id = await connection.QuerySingleAsync<int>(query, new WorkspaceEntity(default, workspace), transaction);

            return new WorkspaceEntity(id, workspace);
        }

        // todo some way of getting to the workspace from the user:
        /*
         *             var membershipQuery = @"
select * from [dbo].[UserWorkspaceMembership] where [UserId] = @id;
";
            var membershipRows = await connection.QueryAsync<UserWorkspaceMembershipRow>(membershipQuery, new { id }, transaction);
            var memberships = membershipRows.GroupBy(r => r.WorkspaceId).Select(
                            g => new WorkspaceMembership(g.Key, g.Select(a => a.Permission).ToArray())
                        ).ToArray();
         * 
         */

        public async Task<WorkspacePermission[]> PermissionsGet(int userId, int workspaceId)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
select * from [dbo].[UserWorkspaceMembership] where [UserId] = @userId and [WorkspaceId] = @workspaceId;
";

            var rows = await connection.QueryAsync<UserWorkspaceMembershipRow>(query, new { userId, workspaceId }, transaction);

            return rows.Select(r => r.Permission).ToArray();
        }

        public async Task<WorkspaceMembership[]> MembershipsGet(int workspaceId)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
select * from [dbo].[UserWorkspaceMembership] where [WorkspaceId] = @workspaceId;
";
            var membershipRows = await connection.QueryAsync<UserWorkspaceMembershipRow>(query, new { workspaceId }, transaction);
            var memberships = membershipRows.GroupBy(r => r.UserId).Select(
                            g => new WorkspaceMembership(workspaceId, g.Key, g.Select(a => a.Permission).ToArray())
                        ).ToArray();

            return memberships;
        }

        public async Task PermissionsCreate(int userId, int workspaceId, WorkspacePermission[] permissions)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
insert into [dbo].[UserWorkspaceMembership] ([UserId], [WorkspaceId], [Permission])
    values (@userId, @workspaceId, @permission);
";
            var rows = permissions.Select(p => new UserWorkspaceMembershipRow(userId, workspaceId, p)).ToArray();

            await connection.ExecuteAsync(query, rows, transaction);
        }
    }
}
