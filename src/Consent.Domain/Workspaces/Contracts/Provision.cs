using System;
using Consent.Domain.Workspaces.Permissions;

namespace Consent.Domain.Workspaces.Contracts;

/*
 * A yes or no choice to accept one or many permissions. May not contain all information required for informed consent
 */
public class Provision
{
    public string Text { get; }
    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(Text));
        }
    }

    public PermissionId[] Permissions { get; }

    public Provision(string text, PermissionId[] permissions)
    {
        ValidateText(text);
        Text = text;

        Permissions = permissions;
    }
}
