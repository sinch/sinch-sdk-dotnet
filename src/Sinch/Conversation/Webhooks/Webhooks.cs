using System;
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

        public Task<Webhook> Create(Webhook request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/webhooks");
            _logger?.LogDebug("Creating a webhook...");
            return _http.Send<Webhook, Webhook>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }
    }
}
