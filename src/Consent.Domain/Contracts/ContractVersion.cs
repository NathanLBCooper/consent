using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Consent.Domain.Contracts;

/*
 * A specific version of a Contract, which contains all wording required for informed consent on one or many provisions 
 */

public class ContractVersion
{
    public ContractVersionId? Id { get; init; }

    private string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        set
        {
            ThrowIfNotDraft();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    // todo is this "Text". Is it something more specific like a introduction?
    private string _text;
    public string Text
    {
        get => _text;
        [MemberNotNull(nameof(_text))]
        set
        {
            ThrowIfNotDraft();
            _text = value;
        }
    }

    private ContractVersionStatus _status = ContractVersionStatus.Draft;
    public ContractVersionStatus Status
    {
        get => _status;
        set
        {
            if (value == Status)
            {
                return;
            }

            if (!Enum.IsDefined(typeof(ContractVersionStatus), value))
            {
                throw new ArgumentException(nameof(Status));
            }

            if (value == ContractVersionStatus.Draft)
            {
                throw new InvalidOperationException("Cannot change a non-draft Version back to draft");
            }

            _status = value;
        }
    }

    public ImmutableList<Provision> Provisions { get; private set; }

    public ContractVersion(string name, string text, ContractVersionStatus status, Provision[] provisions) : this(name, text, provisions)
    {
        Status = status;
    }

    public ContractVersion(string name, string text, Provision[] provisions)
    {
        Name = name;
        Text = text;

        Provisions = provisions.ToImmutableList();
        foreach (var p in provisions)
        {
            p.OnAddedToVersion(this);
        }
    }

    public void AddProvisions(params Provision[] provisions)
    {
        Provisions = Provisions.AddRange(provisions);
        foreach (var p in provisions)
        {
            p.OnAddedToVersion(this);
        }
    }

    private void ThrowIfNotDraft()
    {
        if (Status != ContractVersionStatus.Draft)
        {
            throw new InvalidOperationException("Cannot mutate a non-draft Version");
        }
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
