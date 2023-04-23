﻿using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Consent.Domain.Permissions;

namespace Consent.Domain.Contracts;

/*
 * A yes or no choice to accept one or many permissions. May not contain all information required for informed consent
 */
public class Provision
{
    public ProvisionId? Id { get; init; }

    private ContractVersion? _version;

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

    public ImmutableList<PermissionId> PermissionIds { get; private set; }

    public Provision(string text, params PermissionId[] permissionIds)
    {
        Text = text;
        PermissionIds = permissionIds.ToImmutableList();
    }

    public void OnAddedToVersion(ContractVersion version)
    {
        if (_version != null)
        {
            throw new InvalidOperationException("Provision is already attached to a version");
        }

        _version = version;
    }

    public void AddPermissionIds(params PermissionId[] permissionIds)
    {
        ThrowIfNotDraft();

        PermissionIds = PermissionIds.AddRange(permissionIds);
    }

    private void ThrowIfNotDraft()
    {
        if (_version != null && _version.Status != ContractVersionStatus.Draft)
        {
            throw new InvalidOperationException("Cannot mutate a non-draft Version");
        }
    }
}

public readonly record struct ProvisionId(int Value) : IIdentifier;