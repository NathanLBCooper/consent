using System;
using System.Linq;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ProvisionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_provision_with_empty_name(string text)
    {
        var ctor = () => new Provision(text, Array.Empty<PermissionId>());
        _ = ctor.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_only_change_a_provision_when_contract_version_in_draft()
    {
        var version = new ContractVersionBuilder()
        {
            Provisions = new[] { new Provision("text", new[] { new PermissionId(1010) }) }
        }.Build();
        var provision = version.Provisions.Single();

        provision.AddPermissionIds(new[] { new PermissionId(1011) });
        provision.Text = "new text";

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var addPermissionId = () => { provision.AddPermissionIds(new[] { new PermissionId(1012) }); };
            _ = addPermissionId.ShouldThrow<InvalidOperationException>();
        }, version);

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var changeText = () => { provision.Text = "newer text"; };
            _ = changeText.ShouldThrow<InvalidOperationException>();
        }, version);
    }

    [Fact]
    public void Foo()
    {
        var version = new ContractVersionBuilder().Build();
        var provison = new Provision("text", Array.Empty<PermissionId>());

        // Calling this not from the version is wrong and doesn't actually add to contract. Use domain event or something?
        provison.OnAddedToVersion(version);

        var reattach = () => provison.OnAddedToVersion(version);
        _ = reattach.ShouldThrow<InvalidOperationException>();

        var changeAttached = () => provison.OnAddedToVersion(new ContractVersionBuilder().Build());
        _ = changeAttached.ShouldThrow<InvalidOperationException>();

    }
}
