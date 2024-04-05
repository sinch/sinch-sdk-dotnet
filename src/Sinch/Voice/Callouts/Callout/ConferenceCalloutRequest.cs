namespace Sinch.Voice.Callouts.Callout
{
    public class ConferenceCalloutRequest
    {
        /// <summary>
        ///     The type of device and number or endpoint to call.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Destination Destination { get; set; }
#else
        public Destination Destination { get; set; }
#endif
        /// <summary>
        ///     The conferenceId of the conference to which you want the callee to join. If the conferenceId doesn't exist a
        ///     conference room will be created.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ConferenceId { get; set; }
#else
        public string ConferenceId { get; set; }
#endif
        /// <inheritdoc cref="ConferenceDtmfOptions" />
        public ConferenceDtmfOptions ConferenceDtmfOptions { get; set; }

        /// <summary>
        ///     The number that will be displayed as the icoming caller,
        ///     to set your own CLI, you may use your verified number or your Dashboard virtual number,
        ///     it must be in E.164 format.
        /// </summary>
        public string Cli { get; set; }

        /// <summary>
        ///     When the destination picks up, this DTMF tones will be played to the callee.
        ///     Valid characters in the string are "0"-"9", "#", and "w". A "w" will render a 500 ms pause.
        ///     For example, "ww1234#w#" will render a 1s pause, the DTMF tones "1", "2", "3", "4" and "#" followed
        ///     by a 0.5s pause and finally the DTMF tone for "#". This can be used if the callout destination for
        ///     instance require a conference PIN code or an extension to be entered.
        /// </summary>
        public string Dtmf { get; set; }

        // TODO: add description when updated in docs: https://developers.sinch.com/docs/voice/api-reference/voice/tag/Callouts/#tag/Callouts/operation/Callouts!path=conferenceCallout/maxDuration&t=request
        public int? MaxDuration { get; set; }

        /// <summary>
        ///     If <c>enableAce</c> is set to <c>true</c> and the application has a callback URL specified, you will receive an ACE
        ///     callback when
        ///     the call is answered. When the callback is received, your platform must respond with a svamlet, containing the
        ///     “connectconf” action in order to add the call to a conference or create the conference if it's the first call. If
        ///     it's set to <c>false</c>, no ACE event will be sent to your backend.
        /// </summary>
        public bool? EnableAce { get; set; }

        /// <summary>
        ///     Default: false
        ///     If <c>enableDice</c> is set to <c>true</c> and the application has a callback URL specified, you will receive a
        ///     DiCE callback
        ///     when the call is disconnected. If it's set to <c>false</c>, no DiCE event will be sent to your backend.
        /// </summary>
        public bool? EnableDice { get; set; }

        /// <summary>
        ///     <b>Note:</b> PIE callbacks are not available for DATA Calls; only PSTN and SIP calls. <br /><br />
        ///     If enablePie is set to true and the application has a callback URL specified,
        ///     you will receive a PIE callback after the runMenu action executes and after the configured menu timeout has elapsed
        ///     with no input. If it's set to false, no PIE events will be sent to your backend.
        /// </summary>
        public bool? EnablePie { get; set; }

        /// <summary>
        ///     The voice and language you want to use for the text-to-speech message.
        ///     This can either be defined by the ISO 639 locale and language code or by specifying a particular voice.
        ///     Supported languages and voices are detailed
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/voice-locales/">here</see>.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        ///     The text that will be spoken as a greeting.
        /// </summary>
        public string Greeting { get; set; }

        /// <summary>
        ///     Can be used to input custom data.
        /// </summary>
        public string Custom { get; set; }

        /// <inheritdoc cref="MohClass"/>
        public MohClass MohClass { get; set; }

        /// <summary>
        ///     Can be either pstn for PSTN endpoint or mxp for data (app or web) clients.
        /// </summary>
        public Domain Domain { get; set; }
    }
}
