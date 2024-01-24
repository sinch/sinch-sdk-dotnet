using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Conversations.Create;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Conversations
{
    /// <summary>
    ///     Endpoints for working with the conversation log.
    /// </summary>
    public interface ISinchConversationConversations
    {
        /// <summary>
        /// Creates a new empty conversation. It is generally not needed to create a conversation explicitly since sending or receiving a message automatically creates a new conversation if it does not already exist between the given app and contact. Creating empty conversation is useful if the metadata of the conversation should be populated when the first message in the conversation is a contact message or the first message in the conversation comes out-of-band and needs to be injected with InjectMessage endpoint.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Conversation> Create(CreateConversationRequest request, CancellationToken cancellationToken = default);
    }

    internal class ConversationsClient : ISinchConversationConversations
    {
        private readonly string _projectId;
        private readonly Uri _baseAddress;
        private readonly ILoggerAdapter<ISinchConversationConversations> _logger;
        private readonly IHttp _http;

        public ConversationsClient(string projectId, Uri baseAddress,
            ILoggerAdapter<ISinchConversationConversations> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<Conversation> Create(CreateConversationRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/conversations");
            _logger?.LogDebug("Creating a conversation for {project}", _projectId);
            return _http.Send<CreateConversationRequest, Conversation>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }
    }
}
