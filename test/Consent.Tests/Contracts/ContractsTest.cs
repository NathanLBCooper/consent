using System;
using System.Linq;
using Consent.Domain.Contracts;
using Consent.Domain.Core.Errors;
using Consent.Domain.Purposes;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractsTest
{
    [Fact]
    public void Can_create_and_edit_contract()
    {
        var purposes = new[] { new PurposeId(1) };
        var provision = new ProvisionBuilder(purposes).Build();
        var version = new ContractVersionBuilder()
        {
            Provisions = [provision],
        }.Build().Unwrap();
        var contract = new ContractBuilder()
        {
            Versions = [version]
        }.Build();

        var updatedName = "updated";
        contract.Name = updatedName;

        var newPurpose = new PurposeId(2);
        var updatedVersionName = "updated version";
        version.NameSet(updatedVersionName).Unwrap();
        version.Provisions[0].AddPurposeIds(new[] { newPurpose });

        contract.Name.ShouldBe(updatedName);
        contract.Versions.Single().Name.ShouldBe(updatedVersionName);
        contract.Versions.Single().Provisions[0].PurposeIds.ToArray().ShouldBeEquivalentTo(purposes.Concat(new[] { newPurpose }).ToArray());
    }

    [Fact]
    public void Can_create_contract_and_set_version_to_active()
    {
        var purposes = new[] { new PurposeId(1) };
        var provision = new ProvisionBuilder(purposes).Build();
        var version = new ContractVersionBuilder()
        {
            Provisions = [provision],
        }.Build().Unwrap();
        _ = new ContractBuilder()
        {
            Versions = [version]
        }.Build();

        version.StatusSet(ContractVersionStatus.Active).Unwrap();

        _ = version.NameSet("updated version").UnwrapError().ShouldBeOfType<InvalidOperationError>();
        var updateProvisionInVersion = () => version.Provisions[0].AddPurposeIds(new[] { new PurposeId(2) });
        _ = updateProvisionInVersion.ShouldThrow<InvalidOperationException>();
    }
}
