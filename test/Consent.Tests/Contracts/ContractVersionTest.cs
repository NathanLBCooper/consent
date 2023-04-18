using System;
using System.Linq;
using Consent.Domain.Contracts;
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
    public void Cannot_change_status_to_draft_from_any_other_status()
    {
        var version = new ContractVersionBuilder().Build();

        version.Status = ContractVersionStatus.Draft;

        foreach (var status in NonDraftStatuses)
        {
            version.Status = status;

            var statusChange = () => { version.Status = ContractVersionStatus.Draft; };
            _ = statusChange.ShouldThrow<InvalidOperationException>();
        }

        ForAllNonDraftStatuses(() =>
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
    public void Cannot_change_text_when_not_in_draft()
    {
        var version = new ContractVersionBuilder().Build();
        version.Text = "edited text in draft";

        version.Status = ContractVersionStatus.Active;

        ForAllNonDraftStatuses(() =>
        {
            var statusChange = () => { version.Text = "edited text"; };
            _ = statusChange.ShouldThrow<InvalidOperationException>();
        }, version);
    }

    // todo nothing should be mutable (including contained things) on a non-draft contract

    private static readonly ContractVersionStatus[] NonDraftStatuses =
        Enum.GetValues<ContractVersionStatus>().Where(s => s != ContractVersionStatus.Draft).ToArray();
    private void ForAllNonDraftStatuses(Action action, ContractVersion version)
    {
        foreach (var status in NonDraftStatuses)
        {
            version.Status = status;
            action();
        }
    }
}
