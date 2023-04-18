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

    private readonly string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        private init
        {
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
            if (Status != ContractVersionStatus.Draft)
            {
                throw new InvalidOperationException("Cannot mutate a non-draft Version");
            }

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

    public ImmutableArray<Provision> Provisions { get; private init; }

    public ContractVersion(string name, string text, ContractVersionStatus status, Provision[] provisions) : this(name, text, provisions)
    {
        Status = status;
    }

    public ContractVersion(string name, string text, Provision[] provisions)
    {
        Name = name;
        Text = text;
        Provisions = ImmutableArray.Create(provisions);
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
