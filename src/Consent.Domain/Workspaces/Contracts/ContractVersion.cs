using System;

namespace Consent.Domain.Workspaces.Contracts;

/*
 * An entity that contains all the wording requested for informed consent on one or many provisions 
 */

public class ContractVersion
{
    public string Name { get; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Text { get; }
    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }
    }

    public Provision[] Provisions { get; }

    public ContractVersionStatus Status { get; }
    private static void ValidateStatus(ContractVersionStatus status)
    {
        if (Enum.IsDefined(typeof(ContractVersionStatus), status))
        {
            throw new ArgumentException(nameof(Status));
        }
    }

    public ContractVersion(string name, string text, Provision[] provisions, ContractVersionStatus status)
    {
        ValidateName(name);
        Name = name;

        ValidateText(text);
        Text = text;

        Provisions = provisions;

        ValidateStatus(status);
        Status = status;
    }
}

public record struct ContractVersionId(int Value);

public class ContractVersionEntity : ContractVersion
{
    public ContractVersionId Id { get; private init; }

    public ContractVersionEntity(ContractVersionId id, string name, string text, Provision[] provisions, ContractVersionStatus status) :
        base(name, text, provisions, status)
    {
        Id = id;
    }
}
