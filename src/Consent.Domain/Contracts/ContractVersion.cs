using System;
using System.Collections.Generic;

namespace Consent.Domain.Contracts;

/*
 * A specific version of a Contract, which contains all wording required for informed consent on one or many provisions 
 */

public class ContractVersion
{
    public ContractVersionId? Id { get; init; }

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

    public IReadOnlyCollection<Provision> Provisions { get; }

    public ContractVersionStatus Status { get; }
    private static void ValidateStatus(ContractVersionStatus status)
    {
        if (!Enum.IsDefined(typeof(ContractVersionStatus), status))
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
