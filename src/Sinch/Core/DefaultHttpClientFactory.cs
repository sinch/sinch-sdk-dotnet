using System;
using System.Net.Http;

namespace Sinch.Core
{
    /// <summary>
    ///     Default implementation of IHttpClientFactory for scenarios where DI is not available.
    ///     Creates and manages a single HttpClient instance per client name.
    /// </summary>
    internal sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public DefaultHttpClientFactory()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5), // DNS refresh every 5 min
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10
            };

            _httpClient = new HttpClient(handler);
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
