using Consent.Api.Client.Models.Contracts;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;

namespace Consent.Tests.Builders;
public record UserCreateRequestModelBuilder(string Name = "user")
{
    public UserCreateRequestModel Build()
    {
        return new(Name);
    }
}

public record WorkspaceCreateRequestModelBuilder(string Name = "workspace")
{
    public WorkspaceCreateRequestModel Build()
    {
        return new(Name);
    }
}

public record ContractCreateRequestModelBuilder(int WorkspaceId, string Name = "contract")
{
    public ContractCreateRequestModel Build()
    {
        return new(Name, WorkspaceId);
    }
}

public record ContractVersionCreateRequestModelBuilder()
{
    public ContractVersionCreateRequestModel Build()
    {
        return new();
    }
}
