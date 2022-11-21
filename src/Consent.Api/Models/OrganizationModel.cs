using Consent.Domain.Accounts;

namespace Consent.Api.Models
{
    public record OrganizationModel
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }

    internal static class OrganizationModelMapper
    {
        public static OrganizationModel ToModel(this OrganizationEntity entity)
        {
            return new OrganizationModel { Id = entity.Id, Name = entity.Name };
        }
    }
}
