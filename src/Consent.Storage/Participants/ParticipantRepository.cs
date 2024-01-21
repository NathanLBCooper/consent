using Consent.Application.Participants;
using Consent.Domain.Participants;

namespace Consent.Storage.Participants;

public class ParticipantRepository : Repository<Participant, ParticipantId>, IParticipantRepository
{
    public ParticipantRepository(ParticipantDbContext dbContext) : base(dbContext)
    {
    }
}
