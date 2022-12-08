namespace Consent.Domain.UnitOfWork;

public interface ICreateUnitOfWork
{
    IUnitOfWork Create();
}
