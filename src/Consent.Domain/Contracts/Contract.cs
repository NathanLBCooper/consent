using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract
{
    public ContractId? Id { get; init; }

    public WorkspaceId WorkspaceId { get; private init; }

    private string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    public ImmutableList<ContractVersion> Versions { get; private set; }

    public Contract(string name, WorkspaceId workspaceId, IEnumerable<ContractVersion> versions)
    {
        Name = name;
        WorkspaceId = workspaceId;
        Versions = versions.ToImmutableList();
    }

    public Contract(string name, WorkspaceId workspaceId) : this(name, workspaceId, Array.Empty<ContractVersion>())
    {
    }

    public void AddContractVersions(params ContractVersion[] versions)
    {
        Versions = Versions.AddRange(versions);
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
