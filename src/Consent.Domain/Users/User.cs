using System;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */

public class User
{
    public UserId? Id { get; init; }

    public string Name { get; private set; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public User(string name)
    {
        ValidateName(name);
        Name = name;
    }
}

public readonly record struct UserId(int Value) : IIdentifier;
