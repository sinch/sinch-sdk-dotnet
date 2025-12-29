
using Microsoft.Extensions.Logging;

/// <summary> 
/// Provides a centralized logger factory for Sinch SDK snippets. 
/// </summary> 
public static class LoggerHelper
{
    private static readonly Lazy<ILoggerFactory> _loggerFactory = new(CreateLoggerFactory);

    private static ILoggerFactory Factory => _loggerFactory.Value;

    private static readonly Lazy<ILogger> _logger = new(() => Factory.CreateLogger("Snippet"));

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }

    /// <summary> 
    /// Gets a shared logger instance for snippets. 
    /// </summary> 
    public static ILogger Logger => _logger.Value;
}
