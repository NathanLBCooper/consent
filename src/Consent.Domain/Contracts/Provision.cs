using System;
using System.Collections.Generic;
using Consent.Domain.Permissions;

namespace Consent.Domain.Contracts;

/*
 * A yes or no choice to accept one or many permissions. May not contain all information required for informed consent
 */
public class Provision
{
    public ProvisionId? Id { get; init; }

    public string Text { get; private set; }
    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }
    }

    private readonly List<PermissionId> _permissions;
    public IReadOnlyCollection<PermissionId> Permissions => _permissions;

    public Provision(string text, List<PermissionId> permissions)
    {
        ValidateText(text);
        Text = text;

        _permissions = permissions;
    }

    public Provision(string text) : this(text, new List<PermissionId>())
    {
    }
}

public readonly record struct ProvisionId(int Value) : IIdentifier;
