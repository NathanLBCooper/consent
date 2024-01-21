using Consent.Domain.Participants;

namespace Consent.Application.Participants;

public interface IParticipantRepository : IRepository<Participant, ParticipantId>
{
}
