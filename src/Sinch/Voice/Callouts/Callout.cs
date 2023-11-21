using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Voice.Callouts
{
    /// <summary>
    ///     A callout is a call made to a phone number or app using the API.<br/><br/>
    ///     Makes a call out to a phone number. The types of callouts currently supported are conference callouts,
    ///     text-to-speech callouts, and custom callouts. The custom callout is the most flexible,
    ///     but text-to-speech and conference callouts are more convenient.
    /// </summary>
    public interface ISinchVoiceCallout
    {
        /// <summary>
        ///     The text-to-speech callout calls a phone number and
        ///     plays a synthesized text messages or pre-recorded sound files.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Tts(TtsCalloutRequest request, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///     The conference callout calls a phone number or a user.
        ///     When the call is answered, it's connected to a conference room.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Conference(ConferenceCalloutRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     The custom callout, the server initiates a call from the servers that can be controlled
        ///     by specifying how the call should progress at each call event.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Custom(CustomCalloutRequest request, CancellationToken cancellationToken = default);
    }

    internal class SinchCallout : ISinchVoiceCallout
    {
        private readonly ILoggerAdapter<SinchCallout> _logger;
        private readonly Uri _baseAddress;
        private readonly IHttp _http;

        public SinchCallout(ILoggerAdapter<SinchCallout> logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
        }

        /// <inheritdoc />
        public Task<CalloutResponse> Tts(TtsCalloutRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "calling/v1/callouts");
            _logger?.LogDebug("Making Tts callout request...");
            return _http.Send<object, CalloutResponse>(uri, HttpMethod.Post, new
                {
                    method = CalloutType.Tts.Value,
                    ttsCallout = request
                },
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<CalloutResponse> Conference(ConferenceCalloutRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "calling/v1/callouts");
            _logger?.LogDebug("Making Conference callout request...");
            return _http.Send<object, CalloutResponse>(uri, HttpMethod.Post, new
                {
                    method = CalloutType.Conference.Value,
                    conferenceCallout = request
                },
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<CalloutResponse> Custom(CustomCalloutRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "calling/v1/callouts");
            _logger?.LogDebug("Making Custom callout request...");
            return _http.Send<object, CalloutResponse>(uri, HttpMethod.Post, new
                {
                    method = CalloutType.Custom.Value,
                    customCallout = request
                },
                cancellationToken: cancellationToken);
        }
    }
}
