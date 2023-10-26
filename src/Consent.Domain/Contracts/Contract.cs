using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract : IEntity<ContractId>
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

    private readonly List<ContractVersion> _versions;
    public IReadOnlyList<ContractVersion> Versions => _versions.AsReadOnly();

    public Contract(string name, WorkspaceId workspaceId, IEnumerable<ContractVersion> versions)
    {
        Name = name;
        WorkspaceId = workspaceId;
        _versions = versions.ToList();
    }

    public Contract(string name, WorkspaceId workspaceId) : this(name, workspaceId, Array.Empty<ContractVersion>())
    {
    }

    public void AddContractVersions(params ContractVersion[] versions)
    {
        _versions.AddRange(versions);
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
