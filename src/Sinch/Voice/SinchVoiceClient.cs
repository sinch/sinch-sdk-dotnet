using System;
using System.Net.Http;
using System.Text.Json;
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
        ///     Parse incoming Voice Sinch Events, validate Sinch request signatures, and serialize event responses.
        /// </summary>
        IVoiceSinchEvents SinchEvents { get; }
    }

    /// <inheritdoc />
    internal sealed class SinchVoiceClient : ISinchVoiceClient
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions =
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private const string ConfigRequired =
            "VoiceConfiguration with AppKey and AppSecret is required to use Voice API methods. " +
            "Set VoiceConfiguration when creating SinchClient.";

        private readonly ISinchVoiceCallout? _callouts;
        private readonly ISinchVoiceCalls? _calls;
        private readonly ISinchVoiceConferences? _conferences;
        private readonly ISinchVoiceApplications? _applications;

        internal SinchVoiceClient(
            SinchVoiceConfiguration? config,
            string? voiceUrlOverride,
            string? voiceAppMgmtUrlOverride,
            LoggerFactory? loggerFactory,
            Func<HttpClient> httpClientAccessor)
        {
            ApplicationSignedAuth? auth = null;
            Http? http = null;

            if (config != null)
            {
                if (string.IsNullOrEmpty(config.AppKey))
                    throw new ArgumentNullException(nameof(config.AppKey), "The value should be present");

                if (string.IsNullOrEmpty(config.AppSecret))
                    throw new ArgumentNullException(nameof(config.AppSecret), "The value should be present");

                auth = new ApplicationSignedAuth(config.AppKey, config.AppSecret);
                http = new Http(new Lazy<ISinchAuth>(auth), httpClientAccessor,
                    loggerFactory?.Create<IHttp>(), JsonNamingPolicy.CamelCase);

                var voiceUrl = !string.IsNullOrEmpty(voiceUrlOverride)
                    ? new Uri(voiceUrlOverride)
                    : SinchUrlResolvers.ResolveVoiceUrl(config);

                var voiceAppMgmtUrl = !string.IsNullOrEmpty(voiceAppMgmtUrlOverride)
                    ? new Uri(voiceAppMgmtUrlOverride)
                    : SinchUrlResolvers.ResolveVoiceApplicationManagementUrl(config);

                _callouts = new SinchCallout(loggerFactory?.Create<ISinchVoiceCallout>(), voiceUrl, http);
                _calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), voiceUrl, http);
                _conferences = new SinchConferences(loggerFactory?.Create<ISinchVoiceConferences>(), voiceUrl,
                    http, _callouts);
                _applications = new SinchApplications(loggerFactory?.Create<ISinchVoiceApplications>(),
                    voiceAppMgmtUrl, http);
            }

            SinchEvents = new VoiceSinchEvents(
                http?.JsonSerializerOptions ?? DefaultJsonOptions,
                auth,
                loggerFactory?.Create<IVoiceSinchEvents>());
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts =>
            _callouts ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchVoiceCalls Calls =>
            _calls ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchVoiceConferences Conferences =>
            _conferences ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchVoiceApplications Applications =>
            _applications ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public IVoiceSinchEvents SinchEvents { get; }
    }
}

