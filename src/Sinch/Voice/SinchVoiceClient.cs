using System;
using System.Collections.Generic;
using System.Net.Http;
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

        /// <summary>
        ///     Validates the authentication header of an incoming Sinch event request.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers,
            string body);
    }

    /// <inheritdoc />
    internal sealed class SinchVoiceClient : ISinchVoiceClient
    {
        private readonly ApplicationSignedAuth _applicationSignedAuth;
        private readonly ILoggerAdapter<ISinchVoiceClient>? _logger;

        public SinchVoiceClient(Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http, ApplicationSignedAuth applicationSignedAuth, Uri applicationManagementBaseAddress)
        {
            _applicationSignedAuth = applicationSignedAuth;
            _logger = loggerFactory?.Create<ISinchVoiceClient>();
            Callouts = new SinchCallout(loggerFactory?.Create<ISinchVoiceCallout>(), baseAddress, http);
            Calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), baseAddress, http);
            Conferences = new SinchConferences(loggerFactory?.Create<ISinchVoiceConferences>(), baseAddress, http,
                Callouts);
            Applications = new SinchApplications(loggerFactory?.Create<ISinchVoiceApplications>(),
                applicationManagementBaseAddress, http);
            SinchEvents = new VoiceSinchEvents(http.JsonSerializerOptions,
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

        public bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers, string body)
        {
            return AuthorizationHeaderValidation.Validate(method, path, headers, body, _applicationSignedAuth,
                _logger);
        }
    }
}
