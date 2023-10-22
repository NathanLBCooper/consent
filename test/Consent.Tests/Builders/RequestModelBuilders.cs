using System;
using Consent.Api.Client.Models.Contracts;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;

namespace Consent.Tests.Builders;
public record UserCreateRequestModelBuilder(string Name = "username")
{
    public UserCreateRequestModel Build()
    {
        return new(Name);
    }
}

public record WorkspaceCreateRequestModelBuilder(string Name = "workspacename")
{
    public WorkspaceCreateRequestModel Build()
    {
        return new(Name);
    }
}

public record ContractCreateRequestModelBuilder(int WorkspaceId, string Name = "contractname")
{
    public ContractCreateRequestModel Build()
    {
        return new(Name, WorkspaceId);
    }
}

public record ContractVersionCreateRequestModelBuilder(string Name = "contractversionname", string Text = "contractversiontext")
{
    public ContractVersionCreateRequestModel Build()
    {
        return new(Name, Text);
    }
}

public record ProvisionCreateRequestModelBuilder(int[] Permissions, string Text = "provisiontext")
{
    public ProvisionCreateRequestModel Build()
    {
        return new(Text, Permissions ?? Array.Empty<int>());
    }
}
