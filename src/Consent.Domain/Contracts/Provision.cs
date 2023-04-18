using System;
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

    private string _text;
    public string Text
    {
        get => _text;
        [MemberNotNull(nameof(_text))]
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(Text));
            }

            _text = value;
        }
    }

    public ImmutableArray<PermissionId> Permissions { get; private init; }

    public Provision(string text, params PermissionId[] permissions)
    {
        Text = text;
        Permissions = ImmutableArray.Create(permissions);
    }
}

public readonly record struct ProvisionId(int Value) : IIdentifier;
