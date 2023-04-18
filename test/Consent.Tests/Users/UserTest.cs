using System;
using Consent.Domain.Users;
using Shouldly;

namespace Consent.Tests.Users;

public class UserTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_user_with_empty_name(string name)
    {
        var ctor = () => new User(name);
        _ = ctor.ShouldThrow<ArgumentException>();
    }
}
