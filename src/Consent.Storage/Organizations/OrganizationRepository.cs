using Consent.Domain.Accounts;
using Consent.Storage.UnitOfWork;
using Dapper;
using System.Threading.Tasks;

namespace Consent.Storage.Organizations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IGetConnection _getConnection;

        public OrganizationRepository(IGetConnection getConnection)
        {
            _getConnection = getConnection;
        }

        public async Task<OrganizationEntity?> Get(int id)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
select * from dbo.Organization where Id = @id;
";

            return await connection.QuerySingleOrDefaultAsync<OrganizationEntity?>(query, new { id }, transaction);
        }

        public async Task<OrganizationEntity> Create(Organization organization)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
insert into dbo.Organization (Value) values (@value);
select SCOPE_IDENTITY();
";
            var id = await connection.QuerySingleAsync<int>(query, new { value = new OrganizationEntity(default, organization) }, transaction);

            return new OrganizationEntity(id, organization);
        }
    }
}
