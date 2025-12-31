using System;

namespace Sinch;

/// <summary>
///     Configuration for HTTP client handler behavior.
///     Controls connection pooling, DNS refresh, and connection limits.
/// </summary>
public sealed class HttpClientHandlerConfiguration
{
    /// <summary>
    ///     Gets or sets the lifetime of pooled connections.
    ///     After this time, connections are recreated to pick up DNS changes.
    /// </summary>
    /// <remarks>
    ///     Lower values provide faster DNS updates but more connection overhead.
    ///     Higher values reduce overhead but slower DNS propagation.
    ///     Recommended: 2-10 minutes based on your DNS TTL.
    /// </remarks>
    public TimeSpan? PooledConnectionLifetime { get; set; }

    /// <summary>
    ///     Gets or sets the idle timeout for pooled connections.
    ///     Connections idle longer than this are closed.
    /// </summary>
    /// <remarks>
    ///     Should match or be slightly less than server keep-alive timeout.
    ///     Typical server timeouts are 60-120 seconds.
    /// </remarks>
    public TimeSpan? PooledConnectionIdleTimeout { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of concurrent connections per server.
    /// </summary>
    /// <remarks>
    ///     HTTP/1.1 best practice is 6-10 connections.
    ///     Increase for high-throughput scenarios, decrease for rate-limited APIs.
    /// </remarks>
    public int? MaxConnectionsPerServer { get; set; }

    /// <summary>
    ///     Creates default configuration with recommended values.
    ///     See <see cref="Core.DefaultHttpClientFactory"/> for the actual default values used.
    /// </summary>
    public static HttpClientHandlerConfiguration Default => new()
    {
        PooledConnectionLifetime = Core.DefaultHttpClientFactory.DefaultPooledConnectionLifetime,
        PooledConnectionIdleTimeout = Core.DefaultHttpClientFactory.DefaultPooledConnectionIdleTimeout,
        MaxConnectionsPerServer = Core.DefaultHttpClientFactory.DefaultMaxConnectionsPerServer
    };
}
