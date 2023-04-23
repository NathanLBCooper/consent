using System;
using System.Linq;
using Consent.Domain.Contracts;

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
