using System;
using System.Linq;
using Consent.Domain.Contracts;
using Consent.Domain.Core.Errors;
using Consent.Domain.Purposes;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ProvisionTest
{
    [Theory]
#pragma warning disable xUnit1012 // <Nullable> does not guarantee no nulls
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_provision_with_empty_text(string text)
    {
        var result = Provision.Ctor(text, new[] { new PurposeId(1001) });

        var error = result.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(Provision.Text));
    }

    [Fact]
    public void Can_only_change_a_provision_when_contract_version_in_draft()
    {
        var version = new ContractVersionBuilder()
        {
            Provisions = [Provision.Ctor("text", new[] { new PurposeId(1010) }).Unwrap()]
        }.Build();
        var provision = version.Provisions.Single();

        provision.AddPurposeIds(new[] { new PurposeId(1011) }).Unwrap();
        provision.SetText("new text").Unwrap();

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var result = provision.AddPurposeIds(new[] { new PurposeId(1012) });
            _ = result.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var result = provision.SetText("newer text");
            _ = result.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }

    [Fact]
    public void Provision_can_only_be_attached_to_one_version_once()
    {
        var version = new ContractVersionBuilder().Build();
        var provision = Provision.Ctor("text", new[] { new PurposeId(1002) }).Unwrap();

        // Calling this not from the version is wrong and doesn't actually add to contract. Use domain event or something?
        provision.OnAddedToVersion(version);

        var reattach = () => provision.OnAddedToVersion(version);
        _ = reattach.ShouldThrow<InvalidOperationException>();

        var changeAttached = () => provision.OnAddedToVersion(new ContractVersionBuilder().Build());
        _ = changeAttached.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void Purposes_must_not_be_empty()
    {
        var result = Provision.Ctor("text", Array.Empty<PurposeId>()); // todo null
        var error = result.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(Provision.PurposeIds));
    }
}
