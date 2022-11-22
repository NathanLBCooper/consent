using Consent.Domain.Users;
using Consent.Storage.UnitOfWork;
using Dapper;
using System.Threading.Tasks;

namespace Consent.Storage.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IGetConnection _getConnection;

        public UserRepository(IGetConnection getConnection)
        {
            _getConnection = getConnection;
        }

        public async Task<UserEntity?> Get(int id)
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

            var userQuery = @"
insert into [dbo].[User] ([Name]) values (@name);
select SCOPE_IDENTITY();
";
            var id = await connection.QuerySingleAsync<int>(userQuery, new UserEntity(default, user.Name), transaction);
            return new UserEntity(id, user);
        }
    }
}
