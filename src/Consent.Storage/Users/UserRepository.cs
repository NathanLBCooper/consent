using System.Threading.Tasks;
using Consent.Domain.Users;
using Consent.Storage.UnitOfWork;
using Dapper;

namespace Consent.Storage.Users;

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
        return await connection.QuerySingleOrDefaultAsync<UserEntity?>(userQuery, new { id }, transaction);
    }

    public async Task<UserEntity> Create(User user)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
insert into [dbo].[User] ([Name]) values (@name);
select SCOPE_IDENTITY();
";

        var id = await connection.QuerySingleAsync<UserId>(query, new UserEntity(default, user.Name), transaction);
        return new UserEntity(id, user);
    }
}
