using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Events
{
    /// <summary>
    ///     Endpoint for sending events.
    /// </summary>
    public interface ISinchConversationEvents
    {
        /// <summary>
        ///     Sends an event to the referenced contact from the referenced app. Note that this operation enqueues the event in a queue so a successful response only indicates that the event has been queued.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SendEventResponse> Send(SendEventRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get event from ID
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ConversationEvent> Get(string eventId, CancellationToken cancellationToken = default);
    }

    internal class Events : ISinchConversationEvents
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationEvents> _logger;
        private readonly string _projectId;

        public Events(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationEvents> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<SendEventResponse> Send(SendEventRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/events:send");
            _logger?.LogDebug("Sending an event...");
            return _http.Send<SendEventRequest, SendEventResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ConversationEvent> Get(string eventId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                throw new ArgumentNullException(nameof(eventId));
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/events/{eventId}");
            _logger?.LogDebug("Sending an event...");
            return _http.Send<ConversationEvent>(uri, HttpMethod.Get,
                cancellationToken);
        }
    }
}
