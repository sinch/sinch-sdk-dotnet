using System;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Callouts;
using Sinch.Voice.Calls;

namespace Sinch.Voice
{
    public interface ISinchVoiceClient
    {
        /// <summary>
        ///     A callout is a call made to a phone number or app using the API.
        /// </summary>
        ISinchVoiceCallout Callouts { get; }

        /// <summary>
        ///     Using the Calls endpoint, you can manage on-going calls or retrieve information about a call.
        /// </summary>
        ISinchVoiceCalls Calls { get; }
    }

    internal class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            Callouts = new SinchCallout(loggerFactory?.Create<SinchCallout>(), baseAddress, http);
            Calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), baseAddress, http);
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts { get; }

        public ISinchVoiceCalls Calls { get; }
    }
}
