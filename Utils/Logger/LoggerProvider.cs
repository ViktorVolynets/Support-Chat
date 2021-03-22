using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;

namespace TechnicalSupport.Utils.Logger
{
    public class LoggerProvider : ILoggerProvider
    {
        private string _filePath;
        private string  _connectionString;

        public LoggerProvider(string filePath , string connectionString)
        {
            _filePath = filePath;
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(_filePath , _connectionString);
        }

        public void Dispose()
        {
            
        }
    }
}
