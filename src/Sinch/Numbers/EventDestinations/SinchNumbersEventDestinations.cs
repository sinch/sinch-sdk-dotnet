using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers.EventDestinations
{
    internal sealed class SinchNumbersEventDestinations : ISinchNumbersEventDestinations
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchNumbersEventDestinations>? _logger;
        private readonly string _projectId;

        public SinchNumbersEventDestinations(string projectId, Uri baseAddress,
            ILoggerAdapter<ISinchNumbersEventDestinations>? logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        public Task<EventDestination> Get(CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching event destination for {projectId}", _projectId);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/callbackConfiguration");
            return _http.Send<EventDestination>(uri, HttpMethod.Get, cancellationToken);
        }

        public Task<EventDestination> Update(string hmacSecret, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(hmacSecret))
            {
                throw new ArgumentNullException(nameof(hmacSecret));
            }

            _logger?.LogDebug("Updating event destination for {projectId}", _projectId);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/callbackConfiguration");
            return _http.Send<object, EventDestination>(uri, HttpMethod.Patch, new
            {
                hmacSecret = hmacSecret,
            }, cancellationToken);
        }
    }
}
