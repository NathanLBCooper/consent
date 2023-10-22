using System;
using System.Linq;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractsTest
{
    [Fact]
    public void Can_create_and_edit_contract()
    {
        var permissions = new[] { new PermissionId(1) };
        var provision = new ProvisionBuilder(permissions).Build();
        var version = new ContractVersionBuilder()
        {
            Provisions = new[] { provision },
        }.Build();
        var contract = new ContractBuilder()
        {
            Versions = new[] { version }
        }.Build();

        var updatedName = "updated";
        contract.Name = updatedName;

        var newPermission = new PermissionId(2);
        var updatedVersionName = "updated version";
        version.Name = updatedVersionName;
        version.Provisions[0].AddPermissionIds(new[] { newPermission });

        contract.Name.ShouldBe(updatedName);
        contract.Versions.Single().Name.ShouldBe(updatedVersionName);
        contract.Versions.Single().Provisions[0].PermissionIds.ToArray().ShouldBeEquivalentTo(permissions.Concat(new[] { newPermission }).ToArray());
    }

    [Fact]
    public void Can_create_contract_and_set_version_to_active()
    {
        var permissions = new[] { new PermissionId(1) };
        var provision = new ProvisionBuilder(permissions).Build();
        var version = new ContractVersionBuilder()
        {
            Provisions = new[] { provision },
        }.Build();
        _ = new ContractBuilder()
        {
            Versions = new[] { version }
        }.Build();

        version.Status = ContractVersionStatus.Active;

        var updateVersion = () => version.Name = "updated version";
        var updateProvisionInVersion = () => version.Provisions[0].AddPermissionIds(new[] { new PermissionId(2) });
        _ = updateVersion.ShouldThrow<InvalidOperationException>();
        _ = updateProvisionInVersion.ShouldThrow<InvalidOperationException>();
    }

    // todo maybe these don't have value, depending on the controller tests
}
