using System;

namespace Consent.Domain.Workspaces.Contracts;

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

    public Contract(string name)
    {
        ValidateName(name);
        Name = name;
    }
}

public record struct ContractId(int Value);

public class ContractEntity : Contract
{
    public ContractId Id { get; }

    public ContractEntity(ContractId id, string name) : base(name)
    {
        Id = id;
    }
}
