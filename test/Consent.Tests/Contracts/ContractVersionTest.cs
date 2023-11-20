using System;
using Consent.Domain.Contracts;
using Consent.Domain.Purposes;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractVersionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_contract_version_with_empty_name(string name)
    {
        var ctor = () => new ContractVersionBuilder() { Name = name }.Build();
        _ = ctor.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_add_provision()
    {
        var version = new ContractVersionBuilder().Build();
        var provision = new ProvisionBuilder(new PurposeId(1)).Build();

        version.AddProvisions(provision);

        version.Provisions.ShouldContain(provision);
        provision.ContractVersion.ShouldBe(version);
    }

    [Fact]
    public void Cannot_change_status_to_draft_from_any_other_status()
    {
        var version = new ContractVersionBuilder().Build();

        version.Status = ContractVersionStatus.Draft;

        foreach (var status in Util.NonDraftStatuses)
        {
            version.Status = status;

            var statusChange = () => { version.Status = ContractVersionStatus.Draft; };
            _ = statusChange.ShouldThrow<InvalidOperationException>();
        }

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var statusChange = () => { version.Text = "edited text"; };
            _ = statusChange.ShouldThrow<InvalidOperationException>();
        }, version);
    }

    [Fact]
    public void Cannot_change_status_to_invalid_value()
    {
        var version = new ContractVersionBuilder().Build();

        var statusChange = () => { version.Status = (ContractVersionStatus)199; };
        _ = statusChange.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_only_change_text_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build();
        version.Text = "edited text in draft";

        version.Status = ContractVersionStatus.Active;

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var statusChange = () => { version.Text = "edited text"; };
            _ = statusChange.ShouldThrow<InvalidOperationException>();
        }, version);
    }

    [Fact]
    public void Can_only_change_name_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build();
        version.Name = "edited name in draft";

        version.Status = ContractVersionStatus.Active;

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var nameChange = () => { version.Name = "edited name"; };
            _ = nameChange.ShouldThrow<InvalidOperationException>();
        }, version);
    }
}
