using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Purposes;
using Consent.Domain.Workspaces;

namespace Consent.Tests.Builders;

internal record ContractBuilder
{
    public WorkspaceId WorkspaceId { get; init; } = new WorkspaceId(1);
    public string Name { get; init; } = "my contract";
    public ContractVersion[] Versions { get; init; } = [];

    public Contract Build()
    {
        return new Contract(WorkspaceId, Name, Versions);
    }
}

internal record ContractVersionBuilder
{
    public string Name { get; init; } = "my version";
    public string Text { get; init; } = string.Empty;
    public Provision[] Provisions { get; init; } = [];

    public Result<ContractVersion> Build()
    {
        return ContractVersion.New(Name, Text, Provisions);
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
