using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        Task<Webhook> Create(Webhook request, CancellationToken cancellationToken = default);

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
        Task<IEnumerable<Webhook>> List(string appId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates an existing webhook as specified by the webhook ID.
        /// </summary>
        /// <param name="webhook">Don't forget to provide the ID of the webhook in the object.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Webhook> Update(Webhook webhook, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a webhook as specified by the webhook ID.
        /// </summary>
        /// <param name="webhookId">The unique ID of the webhook.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string webhookId, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal class Webhooks : ISinchConversationWebhooks
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationWebhooks> _logger;
        private readonly string _projectId;

        public Webhooks(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationWebhooks> logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<Webhook> Create(Webhook request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks");
            _logger?.LogDebug("Creating a webhook...");
            return _http.Send<Webhook, Webhook>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Webhook> Get(string webhookId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{webhookId}");
            _logger?.LogDebug("Getting a webhook with {id}...", webhookId);
            return _http.Send<Webhook>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Webhook>> List(string appId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId), "Should have a value");
            }
            
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/apps/{appId}/webhooks");
            _logger?.LogDebug("Listing webhooks for an {appId}...", appId);
            var response = await _http.Send<ListWebhooksResponse>(uri, HttpMethod.Get,
                cancellationToken);
            return response.Webhooks;
        }

        /// <inheritdoc />
        public Task<Webhook> Update(Webhook webhook, CancellationToken cancellationToken = default)
        {
            if (webhook is null)
            {
                throw new ArgumentNullException(nameof(webhook), "Should have a value");
            }

            if (string.IsNullOrEmpty(webhook.Id))
            {
                throw new NullReferenceException($"{nameof(webhook)}.{nameof(webhook.Id)} shouldn't be null");
            }
            
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks/{webhook.Id}");
            _logger?.LogDebug("Updating a webhook with {id}...", webhook.Id);
            return _http.Send<Webhook>(uri, HttpMethod.Patch,
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
            return _http.Send<object>(uri, HttpMethod.Delete,
                cancellationToken);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    internal class ListWebhooksResponse
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        public List<Webhook> Webhooks { get; set; }
    }
}
