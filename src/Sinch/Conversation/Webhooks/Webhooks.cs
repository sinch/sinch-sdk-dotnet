using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.CompilerServices;
using Sinch.Conversation.Hooks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Webhooks
{
    /// <summary>
    ///     Manage your webhooks with this set of methods.
    /// </summary>
    public interface ISinchConversationWebhooks
    {
        /// <summary>
        ///     Creates a webhook for receiving callbacks on specific triggers. You can create up to 5 webhooks per app.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Webhook> Create(CreateWebhookRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a webhook as specified by the webhook ID.
        /// </summary>
        /// <param name="webhookId">The unique ID of the webhook.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Webhook> Get(string webhookId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List all webhooks for a given app as specified by the App ID.
        /// </summary>
        /// <param name="appId">
        ///     The unique ID of the app. You can find this on the [Sinch
        ///     Dashboard](https://dashboard.sinch.com/convapi/apps).
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListWebhooksResponse> List(string appId, CancellationToken cancellationToken = default);

        IAsyncEnumerable<Webhook> ListAuto(string appId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates an existing webhook as specified by the webhook ID.
        /// </summary>
        /// <param name="webhookId">The id of the webhook to update</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Webhook> Update(string webhookId, UpdateWebhookRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a webhook as specified by the webhook ID.
        /// </summary>
        /// <param name="webhookId">The unique ID of the webhook.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string webhookId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Validates callback request.
        /// </summary>
        /// <param name="headers">The callback request headers as single-value entries.</param>
        /// <param name="body">The callback raw body, as received from the HTTP request.</param>
        /// <param name="secret">The webhook secret used to generate the HMAC signature.</param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body, string secret);

        /// <summary>
        ///     Validates callback request.
        /// </summary>
        /// <param name="headers">The callback request headers.</param>
        /// <param name="body">The callback raw body, as received from the HTTP request.</param>
        /// <param name="secret">The webhook secret used to generate the HMAC signature.</param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(IReadOnlyDictionary<string, IEnumerable<string>> headers, string body,
            string secret);

        /// <summary>
        ///     Parses a callback payload from a raw JSON string.
        /// </summary>
        /// <param name="json">The raw callback payload.</param>
        /// <returns>The parsed callback event.</returns>
        ICallbackEvent ParseEvent(string json);

        /// <summary>
        ///     Parses a callback payload from a JSON stream.
        /// </summary>
        /// <param name="json">The callback payload stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The parsed callback event.</returns>
        Task<ICallbackEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal sealed class Webhooks : ISinchConversationWebhooks
    {
        private readonly Uri _baseAddress;
        private readonly Lazy<IHttp> _http;
        private readonly ILoggerAdapter<ISinchConversationWebhooks>? _logger;
        private readonly string _projectId;

        public Webhooks(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationWebhooks>? logger,
            Lazy<IHttp> http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<Webhook> Create(CreateWebhookRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks");
            _logger?.LogDebug("Creating a webhook...");
            return _http.Value.Send<CreateWebhookRequest, Webhook>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Webhook> Get(string webhookId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{webhookId}");
            _logger?.LogDebug("Getting a webhook with {id}...", webhookId);
            return _http.Value.Send<Webhook>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListWebhooksResponse> List(string appId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId), "Should have a value");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/apps/{appId}/webhooks");
            _logger?.LogDebug("Listing webhooks for an {appId}...", appId);
            return _http.Value.Send<ListWebhooksResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<Webhook> ListAuto(string appId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await List(appId, cancellationToken);
            if (response.Webhooks == null)
            {
                yield break;
            }

            foreach (var webhook in response.Webhooks)
            {
                yield return webhook;
            }
        }

        /// <inheritdoc />
        public Task<Webhook> Update(string webhookId, UpdateWebhookRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request), "Should have a value");
            }

            if (string.IsNullOrEmpty(webhookId))
            {
                throw new NullReferenceException($"{nameof(request)}.{nameof(webhookId)} shouldn't be null");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{webhookId}");

            var builder = new UriBuilder(uri);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var propMask = request.GetPropertiesMask();
            if (!string.IsNullOrEmpty(propMask)) queryString.Add("update_mask", propMask);
            builder.Query = queryString.ToString()!;

            _logger?.LogDebug("Updating a webhook with {id}...", webhookId);
            return _http.Value.Send<UpdateWebhookRequest, Webhook>(builder.Uri, HttpMethod.Patch, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string webhookId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(webhookId))
            {
                throw new ArgumentNullException(nameof(webhookId), "Should have a value");
            }

            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{webhookId}");
            _logger?.LogDebug("Deleting a webhook with {id}...", webhookId);
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

        public ICallbackEvent ParseEvent(string json)
        {
            var jsonResult = JsonSerializer.Deserialize<ICallbackEvent>(json, _http.Value.JsonSerializerOptions);
            if (jsonResult == null)
            {
                _logger?.LogError("Failed to deserialize callback event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of callback event failed");
            }

            return jsonResult;
        }

        public async Task<ICallbackEvent> ParseEventAsync(Stream jsonStream,
            CancellationToken cancellationToken = default)
        {
            var jsonResult =
                await JsonSerializer.DeserializeAsync<ICallbackEvent>(jsonStream,
                    SinchConversationClient.JsonSerializerOptionsInner,
                    cancellationToken);
            if (jsonResult == null)
            {
                _logger?.LogError("Failed to deserialize callback event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of callback event failed");
            }

            return jsonResult;
        }
    }
}
