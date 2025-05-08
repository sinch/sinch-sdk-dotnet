using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

    internal sealed class Transcoding : ISinchConversationTranscoding
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationTranscoding>? _logger;
        private readonly string _projectId;

        public Transcoding(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationTranscoding>? logger,
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
