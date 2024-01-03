using System.Text.Json.Serialization;

namespace Sinch.Voice
{
    /// <summary>
    ///     Options to control how DTMF signals are used by the participant in the conference.
    ///     For information on how to use this feature, read more
    ///     <see href="https://developers.sinch.com/docs/voice/api-reference/conference-dtmf">here.</see>
    /// </summary>
    public class ConferenceDtmfOptions
    {
        /// <summary>
        ///     Determines what DTMF mode the participant will use in the call.
        /// </summary>
        [JsonPropertyName("mode")]
        public DtmfMode Mode { get; set; }

        /// <summary>
        ///     The maximum number of accepted digits before sending the collected input via a PIE callback.
        ///     The default value is 1. If the value is greater than 1,
        ///     the PIE callback is triggered by one of the three following events:
        ///     <list type="bullet">
        ///         <item>No additional digit is entered before the <c>timeoutMills</c> timeout period has elapsed.</item>
        ///         <item>The <c>#</c> character is entered.</item>
        ///         <item>The maximum number of digits has been entered.</item>
        ///     </list>
        /// </summary>
        [JsonPropertyName("maxDigits")]
        public int? MaxDigits { get; set; }

        /// <summary>
        ///     The number of milliseconds that the system will wait between entered digits before triggering the PIE callback.
        ///     The default value is <c>3000</c>.
        /// </summary>
        [JsonPropertyName("timeoutMills")]
        public int? TimeoutMills { get; set; }
    }
}
