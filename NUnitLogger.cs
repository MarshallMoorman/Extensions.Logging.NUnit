using System;
using Microsoft.Extensions.Logging;
using NUnitFramework = NUnit.Framework;

namespace Extensions.Logging.NUnit
{
    public class NUnitLogger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _name;

        public NUnitLogger(string name)
            : this(name, filter: null)
        {
        }

        public NUnitLogger(string name, Func<string, LogLevel, bool> filter)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(NUnitLogger) : name;
            _filter = filter;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var runningInNUnitContext = NUnitFramework.TestContext.Progress != null;
            return RunningInNUnitContext() && logLevel != LogLevel.None
                && (_filter == null || _filter(_name, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{ logLevel }: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception.ToString();
            }

            WriteMessage(message, _name);
        }

        private bool RunningInNUnitContext()
        {
            return NUnitFramework.TestContext.Progress != null;
        }

        private void WriteMessage(string message, string name)
        {
            NUnitFramework.TestContext.Progress.WriteLine(message);
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}