using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Purposes;

/*
 *  A specific idea that can be agreed to
 */

public class Purpose : IEntity<PurposeId>
{
    public PurposeId? Id { get; init; }
    public WorkspaceId WorkspaceId { get; private init; }

    public string Name { get; private init; }

    public string Description { get; private init; }

    public static Result<Purpose> Ctor(WorkspaceId workspaceId, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Purpose>.Failure(new ArgumentError(nameof(Name)));
        }

        return Result<Purpose>.Success(new Purpose(workspaceId, name, description));
    }

    private Purpose(WorkspaceId workspaceId, string name, string description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
    }
}

public readonly record struct PurposeId(int Value) : IIdentifier;
