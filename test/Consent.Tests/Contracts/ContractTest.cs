using System;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
using Shouldly;

namespace Consent.Tests.Contracts;

public class ContractTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_contract_with_empty_name(string name)
    {
        var ctor = () => new Contract(name);
        _ = ctor.ShouldThrow<ArgumentException>();
    }

    [Fact] // todo finish and split up, this isn't a test yet
    public void Can_create_contract_and_add_version()
    {
        var unitTestPermissionId = new PermissionId(100);
        var testRunnerPermissionId = new PermissionId(101);
        var remoteRunnerPermissionId = new PermissionId(102);

        var contract = new Contract("Agreement to be unit tested");

        var version = new ContractVersion(
            name: "Version #1",
            text: "The following agreement...",
            status: ContractVersionStatus.Active,
            new[] {
                new Provision("You agree to be unit tested", unitTestPermissionId),
                new Provision("You agree for those results to be shown in the test runner in my IDE", testRunnerPermissionId)
                }
            );

        contract.AddContractVersions(version);

        var expandedVersion = new ContractVersion(
            name: "Version #1",
            text: "The following agreement...",
            new[] {
                new Provision("You agree to be unit tested", unitTestPermissionId),
                new Provision("You agree for those results to be shown in the test runner", testRunnerPermissionId, remoteRunnerPermissionId)
                }
            );

        contract.AddContractVersions(expandedVersion);

        version.Status = ContractVersionStatus.Deprecated;
        expandedVersion.Status = ContractVersionStatus.Active;
    }
}
