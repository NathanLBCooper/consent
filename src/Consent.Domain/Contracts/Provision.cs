using System;
using Consent.Domain.Permissions;

namespace Consent.Domain.Contracts;

/*
 * A yes or no choice to accept one or many permissions. May not contain all information required for informed consent
 */
public class Provision
{
    public ProvisionId? Id { get; init; }

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

public record struct ProvisionId(int Value);
