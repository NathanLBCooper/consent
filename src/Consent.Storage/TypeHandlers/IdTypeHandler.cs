using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Dapper;
using System;
using System.Data;

namespace Consent.Storage.TypeHandlers
{
    internal class UserIdTypeHandler : SqlMapper.TypeHandler<UserId>
    {
        public override UserId Parse(object value)
        {
            // value comes here as a decimal for some reason
            return new UserId(Convert.ToInt32(value));
        }

        public override void SetValue(IDbDataParameter parameter, UserId value)
        {
            parameter.DbType = DbType.Int32;
            parameter.Value = value.Value;
        }
    }

    internal class WorkspaceIdTypeHandler : SqlMapper.TypeHandler<WorkspaceId>
    {
        public override WorkspaceId Parse(object value)
        {
            // value comes here as a decimal for some reason
            return new WorkspaceId(Convert.ToInt32(value));
        }

        public override void SetValue(IDbDataParameter parameter, WorkspaceId value)
        {
            parameter.DbType = DbType.Int32;
            parameter.Value = value.Value;
        }
    }
}
