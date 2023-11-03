using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Numbers.Regions;

namespace Sinch.Numbers
{
    public interface ISinchNumbersRegions
    {
        /// <summary>
        ///     Lists all virtual numbers for a project.
        /// </summary>
        /// <param name="types">
        ///     Only return regions for which numbers are provided with the given types v1: MOBILE, LOCAL or
        ///     TOLL_FREE.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Region>> List(IEnumerable<Types> types,
            CancellationToken cancellationToken = default);
    }

    internal class AvailableRegions : ISinchNumbersRegions
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<AvailableRegions> _logger;
        private readonly string _projectId;

        public AvailableRegions(string projectId, Uri baseAddress,
            ILoggerAdapter<AvailableRegions> loggerAdapter, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = loggerAdapter;
            _http = http;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Region>> List(IEnumerable<Types> types,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching available regions");
            var typesStr = string.Empty;
            if (types is not null)
            {
                var typesQuery = string.Join("&", types.Select(x => "types=" + x.Value));
                if (!string.IsNullOrEmpty(typesQuery))
                {
                    typesStr = typesQuery;
                    _logger?.LogDebug("For {types}", typesStr);
                }
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/availableRegions?{typesStr}");
            var response = await _http.Send<ListRegionsResponse>(uri, HttpMethod.Get, cancellationToken);

            return response.AvailableRegions;
        }
    }
}
