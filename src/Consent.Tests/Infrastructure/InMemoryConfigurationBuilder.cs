using System.Collections.Generic;
using Consent.Storage;

namespace Consent.Tests.Infrastructure;

public record InMemoryConfigurationBuilder()
{
    public SqlSettings SqlSettings { get; init; } = new SqlSettings { ConnectionString = "fakeconnectionstring" };

    public Dictionary<string, string?> Build()
    {
        return new Dictionary<string, string?>
        {
            { "SqlSettings:ConnectionString", SqlSettings.ConnectionString },
        };
    }
}
