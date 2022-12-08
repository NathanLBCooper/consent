using System;

namespace Consent.Domain.Participants;

/**
 *  A person who agrees to things
 */

public record Participant
{
    public string ExternalId { get; private init; }

    public Participant(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException(nameof(ExternalId));
        }

        ExternalId = externalId;
    }
}

public record struct ParticipantId(int Value);

public record ParticipantEntity : Participant
{
    public ParticipantId Id { get; private init; }

    public ParticipantEntity(ParticipantId id, Participant participant) : base(participant)
    {
        Id = id;
    }

    public ParticipantEntity(ParticipantId id, string externalId) : base(externalId)
    {
        Id = id;
    }
}
