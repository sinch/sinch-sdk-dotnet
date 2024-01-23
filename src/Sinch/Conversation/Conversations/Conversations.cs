using System;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Conversations
{
    /// <summary>
    ///     Endpoints for working with the conversation log.
    /// </summary>
    public interface ISinchConversationConversations
    {
        Task<Conversation
    }
    
    internal class Conversations : ISinchConversationConversations
    {
        private readonly string _projectId;
        private readonly Uri _baseAddress;
        private readonly ILoggerAdapter<ISinchConversationConversations> _logger;
        private readonly IHttp _http;

        public Conversations(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationConversations> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }
    }
}
