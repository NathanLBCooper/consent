using System;
using System.Collections.Generic;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract
{
    public ContractId? Id { get; init; }

    public string Name { get; private set; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    private readonly List<ContractVersion> _versions;
    public IReadOnlyCollection<ContractVersion> Versions => _versions;

    public Contract(string name, List<ContractVersion> versions)
    {
        ValidateName(name);
        Name = name;

        _versions = versions;
    }

    public Contract(string name) : this(name, new List<ContractVersion>())
    {
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
