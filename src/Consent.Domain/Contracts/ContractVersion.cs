using System;
using System.Collections.Generic;

namespace Consent.Domain.Contracts;

/*
 * A specific version of a Contract, which contains all wording required for informed consent on one or many provisions 
 */

public class ContractVersion
{
    public ContractVersionId? Id { get; init; }

    public string Name { get; private set; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Text { get; private set; }
    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }
    }

    public ContractVersionStatus Status { get; private set; }
    private static void ValidateStatus(ContractVersionStatus status)
    {
        if (!Enum.IsDefined(typeof(ContractVersionStatus), status))
        {
            throw new ArgumentException(nameof(Status));
        }
    }

    private readonly List<Provision> _provisions;
    public IReadOnlyCollection<Provision> Provisions => _provisions;

    public ContractVersion(string name, string text, ContractVersionStatus status, List<Provision> provisions)
    {
        ValidateName(name);
        Name = name;

        ValidateText(text);
        Text = text;

        ValidateStatus(status);
        Status = status;

        _provisions = provisions;
    }

    public ContractVersion(string name, string text, ContractVersionStatus status) : this(name, text, status, new List<Provision>())
    {
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
