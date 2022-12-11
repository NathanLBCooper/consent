using System;

namespace Consent.Domain.Users;

/**
 *  A user of the system who manages and sets up things
 */

public class User
{
    public string Name { get; }
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

public record struct UserId(int Value);

public class UserEntity : User
{
    public UserId Id { get; }

    public UserEntity(UserId id, string name) : base(name)
    {
        Id = id;
    }
}
