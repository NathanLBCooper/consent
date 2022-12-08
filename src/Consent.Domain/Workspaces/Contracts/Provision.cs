using System;
using Consent.Domain.Workspaces.Permissions;

namespace Consent.Domain.Workspaces.Contracts;

/*
 * A yes or no choice to accept one or many permissions. May not contain all information required for informed consent
 */
public record Provision
{
    public string Text { get; private init; }
    public PermissionId[] Permissions { get; private init; }

    public Provision(string text, PermissionId[] permissions)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }

        Text = text;

        Permissions = permissions;
    }
}
