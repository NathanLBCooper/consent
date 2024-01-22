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
        var ctor = new ContractVersionBuilder() { Name = name }.Build();
        _ = ctor.UnwrapError().ShouldBeOfType<ArgumentError>();
    }

    [Fact]
    public void Can_add_provision()
    {
        var version = new ContractVersionBuilder().Build().Unwrap();
        var provision = new ProvisionBuilder(new PurposeId(1)).Build();

        version.AddProvisions(provision);

        version.Provisions.ShouldContain(provision);
        provision.ContractVersion.ShouldBe(version);
    }

    [Fact]
    public void Cannot_change_status_to_draft_from_any_other_status()
    {
        var version = new ContractVersionBuilder().Build().Unwrap();

        version.StatusSet(ContractVersionStatus.Draft).Unwrap();

        foreach (var status in Util.NonDraftStatuses)
        {
            version.StatusSet(status).Unwrap();

            _ = version.StatusSet(ContractVersionStatus.Draft).UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            _ = version.TextSet("edited text").UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }

    [Fact]
    public void Cannot_change_status_to_invalid_value()
    {
        var version = new ContractVersionBuilder().Build().Unwrap();

        _ = version.StatusSet((ContractVersionStatus)199).UnwrapError().ShouldBeOfType<ArgumentError>();
    }

    [Fact]
    public void Can_only_change_text_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build().Unwrap();
        version.TextSet("edited text in draft").Unwrap();

        version.StatusSet(ContractVersionStatus.Active).Unwrap();

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            _ = version.TextSet("edited text").UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }

    [Fact]
    public void Can_only_change_name_when_in_draft()
    {
        var version = new ContractVersionBuilder().Build().Unwrap();
        version.NameSet("edited name in draft").Unwrap();

        version.StatusSet(ContractVersionStatus.Active).Unwrap();

        Util.InvokeForAllNonDraftStatuses(() =>
        {
            _ = version.NameSet("edited name").UnwrapError().ShouldBeOfType<InvalidOperationError>();
        }, version);
    }
}
