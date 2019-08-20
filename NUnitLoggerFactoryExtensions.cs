using System;
using Extensions.Logging.NUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.Logging
{
    public static class NUnitLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddNUnit(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NUnitLoggerProvider>());

            return builder;
        }

    }
}