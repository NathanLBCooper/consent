using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Participants;

/**
 *  A person who agrees to things
 */

public class Participant : IEntity<ParticipantId>
{
    public ParticipantId? Id { get; init; }

    public string ExternalId { get; private init; }
    private readonly List<Tag> _tags;
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    public static Result<Participant> Ctor(string externalId, IEnumerable<Tag> tags)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return Result<Participant>.Failure(new ArgumentError(nameof(ExternalId)));
        }

        return Result<Participant>.Success(new Participant(externalId, tags.ToList()));
    }

    public static Result<Participant> Ctor(string externalId)
    {
        return Ctor(externalId, new List<Tag>());
    }

    private Participant(string externalId, List<Tag> tags)
    {
        ExternalId = externalId;
        _tags = tags;
    }

    private Participant(string externalId) : this(externalId, new List<Tag>())
    {
    }
}

public readonly record struct ParticipantId(int Value) : IIdentifier;
