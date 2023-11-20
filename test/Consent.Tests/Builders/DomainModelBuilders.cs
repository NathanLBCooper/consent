using System;
using Consent.Domain.Contracts;
using Consent.Domain.Purposes;
using Consent.Domain.Workspaces;

namespace Consent.Tests.Builders;

internal record ContractBuilder
{
    public string Name { get; init; } = "my contract";
    public WorkspaceId WorkspaceId { get; init; } = new WorkspaceId(1);
    public ContractVersion[] Versions { get; init; } = Array.Empty<ContractVersion>();

    public Contract Build()
    {
        return new Contract(Name, WorkspaceId, Versions);
    }
}

internal record ContractVersionBuilder
{
    public string Name { get; init; } = "my version";
    public string Text { get; init; } = string.Empty;
    public Provision[] Provisions { get; init; } = Array.Empty<Provision>();

    public ContractVersion Build()
    {
        return new ContractVersion(Name, Text, Provisions);
    }
}

internal record ProvisionBuilder
{
    public string Text { get; init; } = "my provision";
    public PurposeId[] PurposeIds;

    public ProvisionBuilder(params PurposeId[] purposeIds)
    {
        PurposeIds = purposeIds;
    }

    public Provision Build()
    {
        return new Provision(Text, PurposeIds);
    }
}
