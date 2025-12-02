using System;
using System.Net.Http;

namespace Sinch.Core
{
    /// <summary>
    ///     Default implementation of IHttpClientFactory for scenarios where DI is not available.
    ///     Creates and manages a single HttpClient instance with configured SocketsHttpHandler.
    /// </summary>
    internal sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Default connection lifetime before recreation (DNS refresh interval).
        /// </summary>
        public static readonly TimeSpan DefaultPooledConnectionLifetime = TimeSpan.FromMinutes(5);
        
        /// <summary>
        /// Default idle timeout before closing unused connections.
        /// </summary>
        public static readonly TimeSpan DefaultPooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);
        
        /// <summary>
        /// Default maximum number of concurrent connections per server endpoint.
        /// </summary>
        public const int DefaultMaxConnectionsPerServer = 10;

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

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
