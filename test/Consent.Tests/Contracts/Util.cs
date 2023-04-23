using System;
using System.Linq;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;

namespace Consent.Tests.Contracts;

public static class Util
{
    public static readonly ContractVersionStatus[] NonDraftStatuses =
        Enum.GetValues<ContractVersionStatus>().Where(s => s != ContractVersionStatus.Draft).ToArray();

    public static void InvokeForAllNonDraftStatuses(Action action, ContractVersion version)
    {
        foreach (var status in NonDraftStatuses)
        {
            version.Status = status;
            action();
        }
    }
}

public class ContractBulder
{
    public string Name { get; init; } = "my contract";
    public ContractVersion[] Versions { get; init; } = Array.Empty<ContractVersion>();

    public Contract Build()
    {
        return new Contract(Name, Versions);
    }
}

public class ContractVersionBuilder
{
    public string Name { get; init; } = "my version";
    public string Text { get; init; } = string.Empty;
    public Provision[] Provisions { get; init; } = Array.Empty<Provision>();

    public ContractVersion Build()
    {
        return new ContractVersion(Name, Text, Provisions);
    }
}

public class ProvisionBuilder
{
    public string Text { get; init; } = "my provision";
    public PermissionId[] PermissionIds = Array.Empty<PermissionId>();

    public Provision Build()
    {
        return new Provision(Text, PermissionIds);
    }
}
