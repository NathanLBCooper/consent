using System;
using Consent.Domain.Contracts;

namespace Consent.Tests.Contracts;

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
