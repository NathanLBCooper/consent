using System;
using Consent.Domain.Users;
using Shouldly;

namespace Consent.Tests.Users;

public class UserTest
{
    [Theory]
#pragma warning disable xUnit1012 // <Nullable> does not guarantee no nulls
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_user_with_empty_name(string name)
    {
        var ctor = () => new User(name);
        _ = ctor.ShouldThrow<ArgumentException>();
    }
}
