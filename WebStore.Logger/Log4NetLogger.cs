using System;
using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string categoryName, XmlElement configuration)
        {
            var loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _log = LogManager.GetLogger(loggerRepository.Name, categoryName);

            log4net.Config.XmlConfigurator.Configure(loggerRepository, configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.None => false,
                LogLevel.Trace => _log.IsDebugEnabled,
                LogLevel.Debug => _log.IsDebugEnabled,
                LogLevel.Information => _log.IsInfoEnabled,
                LogLevel.Warning => _log.IsWarnEnabled,
                LogLevel.Error => _log.IsErrorEnabled,
                LogLevel.Critical => _log.IsFatalEnabled,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }

        public void Log<TState>(
            LogLevel level, EventId id,
            TState state, Exception error,
            Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(level)) return;

            var logMessage = formatter(state, error);

            if (string.IsNullOrEmpty(logMessage) && error is null) return;

            switch (level)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(logMessage);
                    break;
                case LogLevel.Information:
                    _log.Info(logMessage);
                    break;
                case LogLevel.Warning:
                    _log.Warn(logMessage);
                    break;
                case LogLevel.Error:
                    _log.Error(logMessage, error);
                    break;
                case LogLevel.Critical:
                    _log.Fatal(logMessage, error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}