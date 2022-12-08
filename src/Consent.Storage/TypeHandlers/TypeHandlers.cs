using Dapper;

namespace Consent.Storage.TypeHandlers;

public static class TypeHandlers
{
    public static void Setup()
    {
        SqlMapper.AddTypeHandler(new UserIdTypeHandler());
        SqlMapper.AddTypeHandler(new WorkspaceIdTypeHandler());
    }
}
