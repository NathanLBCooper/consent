using System;
using System.Collections.Generic;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract
{
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

public record struct ContractId(int Value);

public class ContractEntity : Contract
{
    public ContractId Id { get; }

    public IReadOnlyCollection<ContractVersionEntity> VersionEntities { get; }

    public ContractEntity(ContractId id, string name, ContractVersionEntity[] versions) : base(name, versions)
    {
        Id = id;
        VersionEntities = versions;
    }
}
