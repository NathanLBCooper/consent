using System;
using Consent.Domain.Contracts;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_assign_contract_empty_name(string name)
    {
        var ctor = () => new Contract(name);
        _ = ctor.ShouldThrow<ArgumentException>();

        var contract = new ContractBulder().Build();
        var setter = () => contract.Name = name;
        _ = setter.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_add_version()
    {
        var contract = new ContractBulder().Build();
        var version = new ContractVersionBuilder().Build();

        contract.AddContractVersions(version);

        contract.Versions.ShouldContain(version);
    }
}
