using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Sinch.Conversation.SinchEvents;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.EventDestinations
{
    /// <summary>
    ///     Manage your event destinations with this set of methods.
    /// </summary>
    public interface ISinchConversationEventDestinations
    {
        /// <summary>
        ///     Creates a event destination for receiving event destinations on specific triggers. You can create up to 5 event destinations per app.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Create(CreateEventDestinationRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get an event destination as specified by the event destination ID.
        /// </summary>
        /// <param name="eventDestinationId">The unique ID of the event destination.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Get(string eventDestinationId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List all event destinations for a given app as specified by the App ID.
        /// </summary>
        /// <param name="appId">
        ///     The unique ID of the app. You can find this on the [Sinch
        ///     Dashboard](https://dashboard.sinch.com/convapi/apps).
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListEventDestinationsResponse> List(string appId, CancellationToken cancellationToken = default);

        IAsyncEnumerable<EventDestination> ListAuto(string appId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates an existing event destination as specified by the event destination ID.
        /// </summary>
        /// <param name="eventDestinationId">The id of the event destination to update</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Update(string eventDestinationId, UpdateEventDestinationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes an event destination as specified by the event destination ID.
        /// </summary>
        /// <param name="eventDestinationId">The unique ID of the event destination.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string eventDestinationId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Validates Sinch event request.
        /// </summary>
        /// <param name="headers">The Sinch event request headers as single-value entries.</param>
        /// <param name="body">The Sinch event raw body, as received from the HTTP request.</param>
        /// <param name="secret">The event destination secret used to generate the HMAC signature.</param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body, string secret);

        /// <summary>
        ///     Validates Sinch event request.
        /// </summary>
        /// <param name="headers">The Sinch event request headers.</param>
        /// <param name="body">The Sinch event raw body, as received from the HTTP request.</param>
        /// <param name="secret">The event destination secret used to generate the HMAC signature.</param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(IReadOnlyDictionary<string, IEnumerable<string>> headers, string body,
            string secret);

        /// <summary>
        ///     Parses a Sinch event payload from a raw JSON string.
        /// </summary>
        /// <param name="json">The raw Sinch event payload.</param>
        /// <returns>The parsed Sinch event.</returns>
        IConversationSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Parses a Sinch event payload from a JSON stream.
        /// </summary>
        /// <param name="json">The Sinch event payload stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The parsed Sinch event.</returns>
        Task<IConversationSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal sealed class EventDestinations : ISinchConversationEventDestinations
    {
        private readonly Uri _baseAddress;
        private readonly Lazy<IHttp> _http;
        private readonly ILoggerAdapter<ISinchConversationEventDestinations>? _logger;
        private readonly string _projectId;

        public EventDestinations(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationEventDestinations>? logger,
            Lazy<IHttp> http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<EventDestination> Create(CreateEventDestinationRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks");
            _logger?.LogDebug("Creating a event destination...");
            return _http.Value.Send<CreateEventDestinationRequest, EventDestination>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<EventDestination> Get(string eventDestinationId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{eventDestinationId}");
            _logger?.LogDebug("Getting a event destination with {id}...", eventDestinationId);
            return _http.Value.Send<EventDestination>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListEventDestinationsResponse> List(string appId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId), "Should have a value");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/apps/{appId}/webhooks");
            _logger?.LogDebug("Listing event destination for an {appId}...", appId);
            return _http.Value.Send<ListEventDestinationsResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<EventDestination> ListAuto(string appId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await List(appId, cancellationToken);
            if (response.EventDestinations == null)
            {
                yield break;
            }

            foreach (var eventDestination in response.EventDestinations)
            {
                yield return eventDestination;
            }
        }

        /// <inheritdoc />
        public Task<EventDestination> Update(string eventDestinationId, UpdateEventDestinationRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request), "Should have a value");
            }

            if (string.IsNullOrEmpty(eventDestinationId))
            {
                throw new NullReferenceException($"{nameof(request)}.{nameof(eventDestinationId)} shouldn't be null");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{eventDestinationId}");

            var builder = new UriBuilder(uri);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var propMask = request.GetPropertiesMask();
            if (!string.IsNullOrEmpty(propMask)) queryString.Add("update_mask", propMask);
            builder.Query = queryString.ToString()!;

            _logger?.LogDebug("Updating a event destination with {id}...", eventDestinationId);
            return _http.Value.Send<UpdateEventDestinationRequest, EventDestination>(builder.Uri, HttpMethod.Patch, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string eventDestinationId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(eventDestinationId))
            {
                throw new ArgumentNullException(nameof(eventDestinationId), "Should have a value");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{eventDestinationId}");
            _logger?.LogDebug("Deleting a event destination with {id}...", eventDestinationId);
            return _http.Value.Send<object>(uri, HttpMethod.Delete,
                cancellationToken);
        }

        public bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body,
            string secret)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }

        public bool ValidateAuthenticationHeader(IReadOnlyDictionary<string, IEnumerable<string>> headers, string body,
            string secret)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }

        public IConversationSinchEvent ParseEvent(string json)
        {
            var jsonResult = JsonSerializer.Deserialize<IConversationSinchEvent>(json, _http.Value.JsonSerializerOptions);
            if (jsonResult == null)
            {
                _logger?.LogError("Failed to deserialize conversation Sinch event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of conversation Sinch event failed");
            }

            return jsonResult;
        }

        public async Task<IConversationSinchEvent> ParseEventAsync(Stream jsonStream,
            CancellationToken cancellationToken = default)
        {
            var jsonResult =
                await JsonSerializer.DeserializeAsync<IConversationSinchEvent>(jsonStream,
                    SinchConversationClient.JsonSerializerOptionsInner,
                    cancellationToken);
            if (jsonResult == null)
            {
                _logger?.LogError("Failed to deserialize conversation Sinch event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of conversation Sinch event failed");
            }

            return jsonResult;
        }
    }
}
