using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Messages.Message;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Transcoding
{
    /// <summary>
    ///     Endpoint for transcoding generic message format to channel-specific one.
    /// </summary>
    public interface ISinchConversationTranscoding
    {
        /// <summary>
        ///     Transcodes the message from the Conversation API format to the channel-specific formats for the requested channels. No message is sent to the contact.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TranscodeResponse> Transcode(TranscodeRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    ///     The message to be transcoded, and the app and channels for which the message is to be transcoded.
    /// </summary>
    public class TranscodeRequest
    {
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; }
#endif
        
        /// <summary>
        ///     Message originating from an app
        /// </summary>
#if NET7_0_OR_GREATER
        public required AppMessage AppMessage { get; set; }
#else
        public AppMessage AppMessage { get; set; }
#endif
        
        /// <summary>
        ///     The list of channels for which the message shall be transcoded to.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<ConversationChannel> Channels { get; set; }
#else
        public List<ConversationChannel> Channels { get; set; }
#endif

        /// <summary>
        ///     Optional.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string To { get; set; }
    }

    public class TranscodeResponse
    {
        public Dictionary<ConversationChannel, string> TranscodedMessage { get; set; }
    }

    internal class Transcoding : ISinchConversationTranscoding
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationTranscoding> _logger;
        private readonly string _projectId;

        public Transcoding(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationTranscoding> logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        public Task<TranscodeResponse> Transcode(TranscodeRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages:transcode");
            _logger?.LogDebug("Transcoding a message...");
            return _http.Send<TranscodeRequest, TranscodeResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }
    }
}
