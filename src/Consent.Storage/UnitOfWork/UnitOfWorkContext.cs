using System;
using System.Data;
using Consent.Domain.UnitOfWork;

namespace Consent.Storage.UnitOfWork;

public class UnitOfWorkContext : ICreateUnitOfWork, IGetConnection
{
    private readonly string _connectionString;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkContext(SqlSettings sqlSettings)
    {
        if (sqlSettings == null)
        {
            throw new ArgumentNullException(nameof(sqlSettings));
        }

        if (string.IsNullOrEmpty(sqlSettings.ConnectionString))
        {
            throw new ArgumentException(nameof(sqlSettings.ConnectionString));
        }

        _connectionString = sqlSettings.ConnectionString;
    }

    public (IDbConnection connection, IDbTransaction transaction) GetConnection()
    {
        return !IsUnitOfWorkOpen
            ? throw new InvalidOperationException(
                "There is not current unit of work from which to get a connection. Call Create first")
            : _unitOfWork!.GetConnection();
    }

    public IUnitOfWork Create()
    {
        if (IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "Cannot begin a transaction before the unit of work from the last one is disposed");
        }

        _unitOfWork = new UnitOfWork(_connectionString);
        return _unitOfWork;
    }
}
