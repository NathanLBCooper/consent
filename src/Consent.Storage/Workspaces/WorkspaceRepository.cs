using System.Linq;
using System.Threading.Tasks;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Consent.Storage.UnitOfWork;
using Dapper;

namespace Consent.Storage.Workspaces;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly IGetConnection _getConnection;

    public WorkspaceRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public async Task<WorkspaceEntity?> Get(WorkspaceId id)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
select * from [dbo].[Workspace] where [Id] = @id;
";
        var row = await connection.QuerySingleOrDefaultAsync<WorkspaceRow?>(query, new { id }, transaction);
        if (row == null)
        {
            return null;
        }

        var membership = @"
select * from [dbo].[UserWorkspaceMembership] where [WorkspaceId] = @id;
";
        var membershipRows = await connection.QueryAsync<UserWorkspaceMembershipRow>(membership, new { id }, transaction);
        var memberships = membershipRows.GroupBy(r => r.UserId).Select(
                        g => new WorkspaceMembership(g.Key, g.Select(a => a.Permission).ToArray())
                    ).ToArray();

        return new WorkspaceEntity(row.Id, new Workspace(row.Name, memberships));
    }

    public async Task<WorkspaceEntity> Create(Workspace workspace)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
insert into [dbo].[Workspace] ([Name]) values (@name);
select SCOPE_IDENTITY();
";
        var id = await connection.QuerySingleAsync<WorkspaceId>(query, new WorkspaceEntity(default, workspace), transaction);

        var membershipQuery = @"
        insert into [dbo].[UserWorkspaceMembership] ([UserId], [WorkspaceId], [Permission])
            values (@userId, @workspaceId, @permission);
        ";
        var membershipRows = workspace.Memberships.SelectMany(m => m.Permissions.Select(p => new UserWorkspaceMembershipRow(m.UserId, id, p)));
        _ = await connection.ExecuteAsync(membershipQuery, membershipRows, transaction);

        return new WorkspaceEntity(id, workspace);
    }

    public async Task Update(WorkspaceEntity workspace)
    {
        await Task.CompletedTask;
        throw new System.NotImplementedException();
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
}

internal record WorkspaceRow(WorkspaceId Id, string Name);

internal record UserWorkspaceMembershipRow(UserId UserId, WorkspaceId WorkspaceId, WorkspacePermission Permission);
