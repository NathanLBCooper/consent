using Consent.Domain.Participants;
using Consent.Domain.Users;

namespace Consent.Application.Participants.Get;

public record ParticipantGetQuery(
    ParticipantId ParticipantId,
    UserId RequestedBy
    );
