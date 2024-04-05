using Microsoft.Extensions.Logging;

namespace Sinch.Logger
{
    internal class LoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILoggerAdapter<T> Create<T>()
        {
            var logger = _loggerFactory.CreateLogger<T>();
            return new LoggerAdapter<T>(logger);
        }
    }
}
