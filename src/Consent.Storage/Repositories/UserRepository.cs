using System;
using System.Linq;
using System.Threading.Tasks;
using Consent.Domain.Users;
using Consent.Storage.UnitOfWork;
using Dapper;

namespace Consent.Storage.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IGetConnection _getConnection;

    public UserRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public async Task<UserEntity?> Get(UserId id)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var userQuery = @"
select * from [dbo].[User] where [Id] = @id;
";
        var user = await connection.QuerySingleOrDefaultAsync<UserEntity?>(userQuery, new { id }, transaction);
        if (user == null)
        {
            return null;
        }

        var membership = @"
select * from [dbo].[UserWorkspaceMembership] where [UserId] = @id;
";
        var membershipRows = await connection.QueryAsync<UserWorkspaceMembershipRow>(membership, new { id }, transaction);
        var memberships = membershipRows.GroupBy(r => r.WorkspaceId).Select(
                        g => new WorkspaceMembership(g.Key, g.Select(a => a.Permission).ToArray())
                    ).ToArray();

        return new UserEntity(id, user.Name, memberships);
    }

    public async Task<UserEntity> Create(User user)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
insert into [dbo].[User] ([Name]) values (@name);
select SCOPE_IDENTITY();
";

        var id = await connection.QuerySingleAsync<UserId>(query, user, transaction);
        return new UserEntity(id, user.Name, Array.Empty<WorkspaceMembership>());
    }
}
