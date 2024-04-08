using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Capability
{
    /// <summary>
    ///     A capability query checks the options available for reaching the
    ///     [contact](https://developers.sinch.com/docs/conversation/keyconcepts/#contact)
    ///     on the channels on which it has a channel identity.
    ///     Capability queries can only be executed for contacts that already exist in a project and app.
    ///     For executing the request, either the contact ID or the channel recipient identities of the contact are required.
    ///     The request is executed asynchronously, therefore the service responds immediately.
    ///     The result of the capability query is sent to the registered webhook for the CAPABILITY trigger.
    /// </summary>
    public interface ISinchConversationCapabilities
    {
        /// <summary>
        ///     This method is asynchronous - it immediately returns the requested Capability registration.
        ///     Capability check is then delivered as a callback to registered webhooks with trigger
        ///     CAPABILITY for every reachable channel.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LookupCapabilityResponse> Lookup(LookupCapabilityRequest request,
            CancellationToken cancellationToken = default);
    }

    internal class Capabilities : ISinchConversationCapabilities
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationCapabilities> _logger;
        private readonly string _projectId;

        public Capabilities(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationCapabilities> logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        public Task<LookupCapabilityResponse> Lookup(LookupCapabilityRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/capability:query");
            _logger?.LogDebug("Looking up for a capability...");
            return _http.Send<LookupCapabilityRequest, LookupCapabilityResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }
    }
}
