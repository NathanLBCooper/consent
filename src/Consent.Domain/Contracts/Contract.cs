using System;

namespace Consent.Domain.Contracts;

public record Contract
{
    public string Name { get; private init; }

    public Contract(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }

        Name = name;
    }
}

public record struct ContractId(int Value);

public record ContractEntity : Contract
{
    public ContractId Id { get; private init; }

    public ContractEntity(ContractId id, Contract contract) : base(contract)
    {
        Id = id;
    }

    public ContractEntity(ContractId id, string name) : base(name)
    {
        Id = id;
    }
}
