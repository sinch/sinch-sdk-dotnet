using System;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Callouts;
using Sinch.Voice.Calls;
using Sinch.Voice.Conferences;

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

        /// <summary>
        ///     Using the Conferences endpoint, you can perform tasks like retrieving information about an on-going conference,
        ///     muting or unmuting participants, or removing participants from a conference.
        /// </summary>
        ISinchVoiceConferences Conferences { get; }
    }

    /// <inheritdoc />
    internal class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            Callouts = new SinchCallout(loggerFactory?.Create<SinchCallout>(), baseAddress, http);
            Calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), baseAddress, http);
            Conferences = new SinchConferences(loggerFactory?.Create<ISinchVoiceConferences>(), baseAddress, http);
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts { get; }

        /// <inheritdoc />
        public ISinchVoiceCalls Calls { get; }

        /// <inheritdoc />
        public ISinchVoiceConferences Conferences { get; }
    }
}
