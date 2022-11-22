using Consent.Domain.Users;

namespace Consent.Api.Models
{
    public record UserModel
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }

    internal static class UserModelMapper
    {
        public static UserModel ToModel(this UserEntity entity)
        {
            return new UserModel { Id = entity.Id, Name = entity.Name };
        }
    }
}
