using System;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Callouts;

namespace Sinch.Voice
{
    public interface ISinchVoiceClient
    {
        /// <summary>
        ///     A callout is a call made to a phone number or app using the API.
        /// </summary>
        ISinchVoiceCallout Callouts { get; }
    }

    internal class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            Callouts = new SinchCallout(loggerFactory?.Create<SinchCallout>(), baseAddress, http);
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts { get; }
    }
}
