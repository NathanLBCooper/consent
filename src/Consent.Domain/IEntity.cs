namespace Consent.Domain;

public interface IEntity<TEntityId> where TEntityId : struct, IIdentifier
{
    public TEntityId? Id { get; }
}
