using Consent.Application.Purposes;
using Consent.Domain.Purposes;

namespace Consent.Storage.Purposes;

public class PurposeRepository : Repository<Purpose, PurposeId>, IPurposeRepository
{
    public PurposeRepository(PurposeDbContext dbContext) : base(dbContext)
    {
    }
}
