using System;
using Microsoft.Extensions.Logging;

namespace Consent.Tests;

internal class NullLogger<TCategoryName> : ILogger<TCategoryName>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return new NullScope();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    private class NullScope : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
