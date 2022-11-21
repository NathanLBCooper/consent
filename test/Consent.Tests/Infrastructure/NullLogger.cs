using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consent.Tests.Infrastructure
{
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
}
