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

    public record UserEntity : User, IEntity
    {
        public int Id { get; private init; }

        public UserEntity(int id, User user) : base(user)
        {
            Id = id;
        }

        public UserEntity(int id, string name) : base(name)
        {
            Id = id;
        }
    }
}
