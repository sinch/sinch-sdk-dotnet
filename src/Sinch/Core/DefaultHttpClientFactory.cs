using System;
using System.Net.Http;

namespace Sinch.Core
{
    /// <summary>
    ///     Default implementation of IHttpClientFactory for scenarios where DI is not available.
    ///     Creates and manages a single HttpClient instance with configured SocketsHttpHandler.
    /// </summary>
    internal sealed class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private bool _disposed;
        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Default connection lifetime before recreation (DNS refresh interval).
        ///     Value: 5 minutes.
        /// </summary>
        public static readonly TimeSpan DefaultPooledConnectionLifetime = TimeSpan.FromMinutes(5);

        /// <summary>
        ///     Default idle timeout before closing unused connections.
        ///     Value: 2 minutes.
        /// </summary>
        public static readonly TimeSpan DefaultPooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);

        /// <summary>
        ///     Default maximum number of concurrent connections per server endpoint.
        ///     Value: 10 connections.
        /// </summary>
        public const int DefaultMaxConnectionsPerServer = 10;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHttpClientFactory"/> class.
        /// </summary>
        /// <param name="configuration">
        ///     Optional HTTP client handler configuration. If <c>null</c>, 
        ///     <see cref="HttpClientHandlerConfiguration.Default"/> is used.
        /// </param>
        public DefaultHttpClientFactory(HttpClientHandlerConfiguration? configuration = null)
        {
            var config = configuration ?? HttpClientHandlerConfiguration.Default;

            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = config.PooledConnectionLifetime ?? DefaultPooledConnectionLifetime,
                PooledConnectionIdleTimeout = config.PooledConnectionIdleTimeout ?? DefaultPooledConnectionIdleTimeout,
                MaxConnectionsPerServer = config.MaxConnectionsPerServer ?? DefaultMaxConnectionsPerServer
            };

            _httpClient = new HttpClient(handler);
        }

        /// <inheritdoc />
        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            _httpClient.Dispose();
            _disposed = true;
        }
    }
}
