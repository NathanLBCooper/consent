using System;
using Consent.Domain.Core;
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
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Description { get; private init; }

    public static Result<Purpose> Ctor(WorkspaceId workspaceId, string name, string description)
    {
        ValidateName(name);

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
