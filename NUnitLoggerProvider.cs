using System;
using Microsoft.Extensions.Logging;
using Extensions.Logging.NUnit;

namespace Extensions.Logging.NUnit
{
    [ProviderAlias("NUnit")]
    public class NUnitLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        
        public NUnitLoggerProvider()
        {
            _filter = null;
        }
        public ILogger CreateLogger(string name)
        {
            return new NUnitLogger(name, _filter);
        }

        public void Dispose()
        {
            
        }
    }
}