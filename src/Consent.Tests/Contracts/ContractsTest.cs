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
            Provisions = new[] { provision },
        }.Build();
        var contract = new ContractBuilder()
        {
            Versions = new[] { version }
        }.Build();

        var updatedName = "updated";
        contract.SetName(updatedName).Unwrap();

        var newPurpose = new PurposeId(2);
        var updatedVersionName = "updated version";
        version.SetName(updatedVersionName).Unwrap();
        version.Provisions[0].AddPurposeIds(new[] { newPurpose }).Unwrap();

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
            Provisions = new[] { provision },
        }.Build();
        _ = new ContractBuilder()
        {
            Versions = new[] { version }
        }.Build();

        version.SetStatus(ContractVersionStatus.Active).Unwrap();

        var updateVersionResult = version.SetName("updated version");
        var updateProvisionInVersion = version.Provisions[0].AddPurposeIds(new[] { new PurposeId(2) });
        _ = updateVersionResult.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        _ = updateProvisionInVersion.UnwrapError().ShouldBeOfType<InvalidOperationError>();
    }

    // todo maybe these don't have value, depending on the controller tests
}
