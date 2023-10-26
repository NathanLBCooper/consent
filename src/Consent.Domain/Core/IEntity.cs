namespace Consent.Domain.Core;

public interface IEntity<TEntityId> where TEntityId : struct, IIdentifier
{
    public TEntityId? Id { get; }
}
