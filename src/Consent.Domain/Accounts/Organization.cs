namespace Consent.Domain.Accounts
{
    public record Organization
    {
        public string Name { get; init; }

        public Organization(string name)
        {
            Name = name;
        }
    }

    public record OrganizationEntity : Organization, IEntity
    {
        public int Id { get; init; }

        public OrganizationEntity(int id, Organization organization) : base(organization)
        {
            Id = id;
        }
    }
}
