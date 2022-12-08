using System.Data;

namespace Consent.Storage.UnitOfWork;

public interface IGetConnection
{
    (IDbConnection connection, IDbTransaction transaction) GetConnection();
}
