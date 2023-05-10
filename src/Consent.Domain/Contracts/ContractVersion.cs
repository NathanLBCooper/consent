using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

    private readonly List<Provision> _provisions;
    public IReadOnlyList<Provision> Provisions => _provisions.AsReadOnly();

    public ContractVersion(string name, string text, IEnumerable<Provision> provisions)
    {
        Name = name;
        Text = text;

        _provisions = provisions.ToList();
        foreach (var p in provisions)
        {
            p.OnAddedToVersion(this);
        }
    }

    private ContractVersion(string name, string text, ContractVersionStatus status) : this(name, text, Array.Empty<Provision>())
    {
        Status = status;
    }

    public void AddProvisions(params Provision[] provisions)
    {
        _provisions.AddRange(provisions);
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
