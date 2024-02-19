using System;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Events
{
    /// <summary>
    ///     Service for sending events.
    /// </summary>
    public interface ISinchConversationEvents
    {
        Task<SendEventResponse> Send(SendEventRequest request, CancellationToken cancellationToken = default);
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
        
        public Task<SendEventResponse> Send(SendEventRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
