using Consent.Domain.Contracts;
using Consent.Domain.Core.Errors;
using Consent.Domain.Workspaces;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractTest
{
    [Theory]
#pragma warning disable xUnit1012 // <Nullable> does not guarantee no nulls
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_assign_contract_empty_name(string name)
    {
        var result = Contract.Ctor(new WorkspaceId(1), name);
        var error = result.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(Contract.Name));

        var contract = new ContractBuilder().Build();
        var setResult = contract.SetName(name);
        var setError = setResult.UnwrapError().ShouldBeOfType<ArgumentError>();
        setError.ParamName.ShouldBe(nameof(Contract.Name));
    }

    [Fact]
    public void Can_add_version()
    {
        var contract = new ContractBuilder().Build();
        var version = new ContractVersionBuilder().Build();

        contract.AddContractVersions(version);

        contract.Versions.ShouldContain(version);
    }
}
