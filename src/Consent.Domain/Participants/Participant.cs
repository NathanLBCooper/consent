using System;

namespace Consent.Domain.Participants;

/**
 *  A person who agrees to things
 */

public class Participant
{
    public ParticipantId? Id { get; init; }

    public string ExternalId { get; private set; }
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

public readonly record struct ParticipantId(int Value) : IIdentifier;
