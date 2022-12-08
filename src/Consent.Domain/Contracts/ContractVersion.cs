using System;

namespace Consent.Domain.Contracts;

/*
 * An entity that contains all the wording requested for informed consent on one or many provisions 
 */

public record ContractVersion
{
    public string Name { get; private init; }
    public string Text { get; private init; }
    public Provision[] Provisions { get; private init; }
    public ContractVersionStatus Status { get; private init; }

    public ContractVersion(string name, string text, Provision[] provisions, ContractVersionStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }

        Name = name;

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }

        Text = text;

        Provisions = provisions;

        if (Enum.IsDefined(typeof(ContractVersionStatus), status))
        {
            throw new ArgumentException(nameof(Status));
        }

        Status = status;
    }
}

public record struct ContractVersionId(int Value);

public record ContractVersionEntity : ContractVersion
{
    public ContractVersionId Id { get; private init; }

    public ContractVersionEntity(ContractVersionId id, ContractVersion contract) : base(contract)
    {
        Id = id;
    }

    public ContractVersionEntity(ContractVersionId id, string name, string text, Provision[] provisions, ContractVersionStatus status) :
        base(name, text, provisions, status)
    {
        Id = id;
    }
}
