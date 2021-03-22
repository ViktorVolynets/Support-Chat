using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TechnicalSupport.Data;
using TechnicalSupport.Models.ServiceModels;

namespace TechnicalSupport.Utils.Logger
{

    public class DbLogger : ILogger
    {
        private readonly DbContextOptionsBuilder<ChatServiceContext> _DbOptionsBuilder;
        private static string _path;
        private static object _lock = new object();


        public DbLogger(string path, string connectionString)
        {
            _path = path;

            _DbOptionsBuilder = new DbContextOptionsBuilder<ChatServiceContext>()
                .UseSqlServer(connectionString);
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }


        //Write log only if error occure
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error || logLevel == LogLevel.Critical;
        }


        //write logs in db and dublicate them in specified file
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            if (formatter == null) return;

            var logMessage = DateTime.UtcNow + "  " + formatter(state, exception) + Environment.NewLine;

            if (IsEnabled(logLevel))
            {
                WriteTechnicalLog(logMessage, exception);
            }
            else
            {
                WriteTraceLog(logMessage);
            }
        }


        /// <summary>
        /// Is used to store trace logs
        /// </summary>
        private async void WriteTraceLog(string logMessage)
        {
            using var _db = new ChatServiceContext(_DbOptionsBuilder.Options);

            await File.AppendAllTextAsync(_path, logMessage);

            var log = new TraceLog
            {
                LogMessage = logMessage,
                Time = DateTime.UtcNow
            };

            _db.TraceLogs.Add(log);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// Is used to store error/critical logs
        /// </summary>
        private async void WriteTechnicalLog(string logMessage , Exception e)
        {
            if (e == null) return;
            using var _db = new ChatServiceContext(_DbOptionsBuilder.Options);

            await File.AppendAllTextAsync(_path, logMessage);

            if (e==null)
            {
                return;
            }
            var log = new ErrorLog
            {
                Code = e.HResult,
                LogMessage = logMessage,
                Time = DateTime.UtcNow,
                StackTrace = e.StackTrace
            };

            _db.TechnicalLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}