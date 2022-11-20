using Consent.Domain.Accounts;
using Consent.Storage.UnitOfWork;
using Dapper;
using System.Threading.Tasks;

namespace Consent.Storage.Organizations
{
    public class OrganizationRepository
    {
        private readonly IGetConnection _getConnection;

        public OrganizationRepository(IGetConnection getConnection)
        {
            _getConnection = getConnection;
        }

        public async Task<OrganizationEntity> Create(Organization organization)
        {
            var (connection, transaction) = _getConnection.GetConnection();

            var query = @"
insert into Organization (Value) values (@value);
select SCOPE_IDENTITY();
";
            var id = await connection.QuerySingleAsync<int>(query, new { value = new OrganizationEntity(default, organization) }, transaction);

            return new OrganizationEntity(id, organization);
        }
    }
}
