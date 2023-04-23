using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract
{
    public ContractId? Id { get; init; }

    private readonly string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        private init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    public ImmutableList<ContractVersion> Versions { get; private set; }

    public Contract(string name, ContractVersion[] versions)
    {
        Name = name;
        Versions = versions.ToImmutableList();
    }

    public Contract(string name) : this(name, Array.Empty<ContractVersion>())
    {
    }

    public void AddContractVersions(params ContractVersion[] versions)
    {
        Versions = Versions.AddRange(versions);
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
