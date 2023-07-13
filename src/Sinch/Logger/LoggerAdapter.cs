using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Sinch.Logger
{
    [SuppressMessage("Usage", "CA2254:Template should be a static expression")]
    internal class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message);
            }
        }

        public void LogInformation<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, arg0);
            }
        }

        public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, arg0, arg1);
            }
        }

        public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, arg0, arg1, arg2);
            }
        }

        public void LogError(string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message);
            }
        }

        public void LogError<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message, arg0);
            }
        }

        public void LogError<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message, arg0, arg1);
            }
        }

        public void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message, arg0, arg1, arg2);
            }
        }

        public void LogDebug(string message)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message);
            }
        }

        public void LogDebug<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, arg0);
            }
        }

        public void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, arg0, arg1);
            }
        }

        public void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, arg0, arg1, arg2);
            }
        }

        public void LogTrace(string message)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(message);
            }
        }

        public void LogTrace<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(message, arg0);
            }
        }

        public void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(message, arg0, arg1);
            }
        }

        public void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(message, arg0, arg1, arg2);
            }
        }

        public void LogWarning(string message)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message);
            }
        }

        public void LogWarning<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, arg0);
            }
        }

        public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, arg0, arg1);
            }
        }

        public void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, arg0, arg1, arg2);
            }
        }
    }

// ReSharper disable once UnusedTypeParameter
    internal interface ILoggerAdapter<T>
    {
        void LogInformation(string message);
        void LogInformation<T0>(string message, T0 arg0);
        void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);

        void LogError(string message);
        void LogError<T0>(string message, T0 arg0);
        void LogError<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);

        void LogDebug(string message);
        void LogDebug<T0>(string message, T0 arg0);
        void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);

        void LogTrace(string message);
        void LogTrace<T0>(string message, T0 arg0);
        void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);

        void LogWarning(string message);
        void LogWarning<T0>(string message, T0 arg0);
        void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    }
}
