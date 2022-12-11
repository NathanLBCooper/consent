using System;

namespace Consent.Domain.Participants;

/**
 *  A person who agrees to things
 */

public class Participant
{
    public string ExternalId { get; }
    private static void ValidateExternalId(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException(nameof(ExternalId));
        }
    }

    public Participant(string externalId)
    {
        ValidateExternalId(externalId);
        ExternalId = externalId;
    }
}

public record struct ParticipantId(int Value);

public class ParticipantEntity : Participant
{
    public ParticipantId Id { get; }

    public ParticipantEntity(ParticipantId id, string externalId) : base(externalId)
    {
        Id = id;
    }
}
