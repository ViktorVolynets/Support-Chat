using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;

namespace TechnicalSupport.Utils.Logger
{
    public static class Logger
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory , string filePath , string connectionString)
        {
            factory.AddProvider(new LoggerProvider(filePath, connectionString));

            return factory;
        }
    }
}
