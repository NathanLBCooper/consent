using System.Threading.Tasks;

namespace Consent.Domain.Accounts
{
    public interface IOrganizationRepository
    {
        Task<OrganizationEntity?> Get(int id);
        Task<OrganizationEntity> Create(Organization organization);
    }
}
