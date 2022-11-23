using System;

namespace Consent.Domain.Users
{
    public record User
    {
        public string Name { get; private init; }

        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(Name));
            Name = name;
        }
    }

    public record struct UserId(int Value);

    public record UserEntity : User
    {
        public UserId Id { get; private init; }

        public UserEntity(UserId id, User user) : base(user)
        {
            Id = id;
        }

        public UserEntity(UserId id, string name) : base(name)
        {
            Id = id;
        }
    }
}
