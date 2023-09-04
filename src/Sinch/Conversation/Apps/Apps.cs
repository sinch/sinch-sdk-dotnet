using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Apps.Create;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Apps are created and configured through
    ///     the <see href="https://dashboard.sinch.com/convapi/getting-started">Sinch Dashboard</see>,
    ///     are tied to the API user and come with a set of channel credentials
    ///     for each underlying connected channel.
    ///     The app has a list of conversations between itself and different contacts which share the same project.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        ///     You can create a new Conversation API app using the API.
        ///     You can create an app for one or more channels at once.
        ///     The ID of the app is generated at creation and will be returned in the response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<App> Create(Request request, CancellationToken cancellationToken = default);
    }
    
    internal class Apps : IApp
    {
        private readonly string _projectId;
        private readonly Uri _baseAddress;
        private readonly ILoggerAdapter<Apps> _logger;
        private readonly IHttp _http;

        public Apps(string projectId, Uri baseAddress, ILoggerAdapter<Apps> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        public Task<App> Create(Request request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps");
            _logger?.LogDebug("Creating an app...");
            return _http.Send<Request, App>(uri, HttpMethod.Post, request, cancellationToken);
        }
    }
}
