using Consent.Domain.Contracts;
using Consent.Domain.Core.Errors;
using Consent.Domain.Purposes;
using Consent.Tests.Builders;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractVersionTest
{
    [Theory]
#pragma warning disable xUnit1012 // <Nullable> does not guarantee no nulls
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_contract_version_with_empty_name(string name)
    {
        var result = ContractVersion.Ctor(name, "text", []);
        _ = result.UnwrapError().ShouldBeOfType<ArgumentError>();
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

        version.SetStatus(ContractVersionStatus.Draft).Unwrap();

        foreach (var status in Util.NonDraftStatuses)
        {
            version.SetStatus(status).Unwrap();

            var statusChangeResult = version.SetStatus(ContractVersionStatus.Draft);
            _ = statusChangeResult.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }
    }

    [Fact]
    public void Cannot_change_status_to_invalid_value()
    {
        var version = new ContractVersionBuilder().Build();

        var statusChangeResult = version.SetStatus((ContractVersionStatus)199);
        var error = statusChangeResult.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(ContractVersion.Status));
    }

    [Fact]
    public void Can_only_change_text_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build();
        version.SetText("edited text in draft").Unwrap();

        version.SetStatus(ContractVersionStatus.Active).Unwrap();

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var statusChangeResult = version.SetText("edited text");
            _ = statusChangeResult.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }

    [Fact]
    public void Can_only_change_name_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build();
        version.SetName("edited name in draft").Unwrap();

        version.SetStatus(ContractVersionStatus.Active).Unwrap();

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            var nameChangeResult = version.SetName("edited name");
            _ = nameChangeResult.UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }
}
