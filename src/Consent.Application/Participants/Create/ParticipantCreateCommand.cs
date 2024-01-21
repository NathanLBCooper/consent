using Consent.Domain.Users;

namespace Consent.Application.Participants.Create;

public record ParticipantCreateCommand(
    UserId RequestedBy
    );
