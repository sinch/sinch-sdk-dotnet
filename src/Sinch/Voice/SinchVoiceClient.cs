using System;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Applications;
using Sinch.Voice.Callouts;
using Sinch.Voice.Calls;
using Sinch.Voice.Conferences;
using Sinch.Voice.SinchEvents;

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

        /// <summary>
        ///     You can use the API to manage features of applications in your project.
        /// </summary>
        ISinchVoiceApplications Applications { get; }

        /// <summary>
        ///     Parse and validate incoming Voice Sinch event payloads.
        /// </summary>
        IVoiceSinchEvents SinchEvents { get; }
    }

    /// <inheritdoc />
    internal sealed class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http, ApplicationSignedAuth applicationSignedAuth, Uri applicationManagementBaseAddress)
        {
            Callouts = new SinchCallout(loggerFactory?.Create<ISinchVoiceCallout>(), baseAddress, http);
            Calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), baseAddress, http);
            Conferences = new SinchConferences(loggerFactory?.Create<ISinchVoiceConferences>(), baseAddress, http,
                Callouts);
            Applications = new SinchApplications(loggerFactory?.Create<ISinchVoiceApplications>(),
                applicationManagementBaseAddress, http);
            SinchEvents = new VoiceSinchEvents(http.JsonSerializerOptions, applicationSignedAuth,
                loggerFactory?.Create<IVoiceSinchEvents>());
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts { get; }

        /// <inheritdoc />
        public ISinchVoiceCalls Calls { get; }

        /// <inheritdoc />
        public ISinchVoiceConferences Conferences { get; }

        /// <inheritdoc />
        public ISinchVoiceApplications Applications { get; }

        /// <inheritdoc />
        public IVoiceSinchEvents SinchEvents { get; }
    }
}
