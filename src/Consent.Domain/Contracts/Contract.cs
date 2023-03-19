using System;
using System.Collections.Generic;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract
{
    public ContractId? Id { get; init; }

    public string Name { get; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public IReadOnlyCollection<ContractVersion> Versions { get; }

    public Contract(string name, ContractVersion[] versions)
    {
        ValidateName(name);
        Name = name;

        Versions = versions;
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
