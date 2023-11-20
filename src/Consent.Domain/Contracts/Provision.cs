using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Consent.Domain.Core;
using Consent.Domain.Purposes;

namespace Consent.Domain.Contracts;

/*
 * A yes or no choice to accept one or many purposes. May not contain all information required for informed consent
 */
public class Provision
{
    public ProvisionId? Id { get; init; }

    public ContractVersion? ContractVersion { get; private set; }

    private string _text;
    public string Text
    {
        get => _text;
        [MemberNotNull(nameof(_text))]
        set
        {
            ThrowIfNotDraft();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Text));
            }

            _text = value;
        }
    }

    private ImmutableList<PurposeId> _purposeIds;
    public ImmutableList<PurposeId> PurposeIds
    {
        get => _purposeIds;
        [MemberNotNull(nameof(_purposeIds))]
        private set
        {
            if (value.IsEmpty)
            {
                throw new ArgumentException("Cannot be empty", nameof(PurposeIds));
            }

            _purposeIds = value;
        }
    }

    public Provision(string text, IEnumerable<PurposeId> purposeIds)
    {
        Text = text;
        PurposeIds = purposeIds.ToImmutableList();
    }

    private Provision(string text)
    {
        Text = text;
        _purposeIds = [];
    }

    public void OnAddedToVersion(ContractVersion version)
    {
        if (ContractVersion is not null)
        {
            throw new InvalidOperationException("Provision is already attached to a version");
        }

        ContractVersion = version;
    }

    public void AddPurposeIds(IEnumerable<PurposeId> purposeIds)
    {
        ThrowIfNotDraft();

        PurposeIds = PurposeIds.AddRange(purposeIds);
    }

    private void ThrowIfNotDraft()
    {
        if (ContractVersion is not null && ContractVersion.Status != ContractVersionStatus.Draft)
        {
            throw new InvalidOperationException("Cannot mutate a non-draft Version");
        }
    }
}

public readonly record struct ProvisionId(int Value) : IIdentifier;
