using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;

namespace Consent.Domain.Participants;

/**
 *  A person who agrees to things
 */

public class Participant : IEntity<ParticipantId>
{
    public ParticipantId? Id { get; init; }

    public string ExternalId { get; private init; }
    private static void ValidateExternalId(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException(nameof(ExternalId));
        }
    }

    private readonly List<Tag> _tags;
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    public static Result<Participant> Ctor(string externalId, IEnumerable<Tag> tags)
    {
        ValidateExternalId(externalId);
        return Result<Participant>.Success(new Participant(externalId, tags.ToList()));
    }

    public static Result<Participant> Ctor(string externalId)
    {
        return Ctor(externalId, new List<Tag>());
    }

    private Participant(string externalId, List<Tag> tags)
    {
        ValidateExternalId(externalId);
        ExternalId = externalId;
        _tags = tags;
    }

    private Participant(string externalId) : this(externalId, new List<Tag>())
    {
    }
}

public readonly record struct ParticipantId(int Value) : IIdentifier;
