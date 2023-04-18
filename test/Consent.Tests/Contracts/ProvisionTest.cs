using System;
using Consent.Domain.Contracts;
using Consent.Domain.Permissions;
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
}
