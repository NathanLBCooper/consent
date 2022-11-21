using Consent.Domain.UnitOfWork;
using System.Threading.Tasks;

namespace Consent.Domain.Accounts
{
    public interface IOrganizationEndpoint
    {
        Task<OrganizationEntity?> Get(int id);
        Task<OrganizationEntity> Create(string name);
    }

    public class OrganizationEndpoint : IOrganizationEndpoint
    {
        // todo this isn't just a pass-through to the repo. Check things, like can this user do or see this

        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICreateUnitOfWork _createUnitOfWork;

        public OrganizationEndpoint(IOrganizationRepository organizationRepository, ICreateUnitOfWork createUnitOfWork)
        {
            _organizationRepository = organizationRepository;
            _createUnitOfWork = createUnitOfWork;
        }

        public async Task<OrganizationEntity?> Get(int id)
        {
            using var uow = _createUnitOfWork.Create();

            return await _organizationRepository.Get(id);
        }

        public async Task<OrganizationEntity> Create(string name)
        {
            using var uow = _createUnitOfWork.Create();

            var organization = new Organization(name);

            return await _organizationRepository.Create(organization);
        }
    }
}
