using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Verification.Hooks
{
    public class SmsRequestEventResponse : RequestEventResponseBase
    {
        [JsonPropertyName("sms")]
        public Sms Sms { get; set; }
    }

    public class Sms
    {
        /// <summary>
        ///     The SMS PIN that should be used.
        ///     By default, the Sinch dashboard will automatically generate PIN codes for SMS verification.
        ///     If you want to set your own PIN, you can specify it in the response to the Verification Request Event.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        ///     The SMS verification content language. Set in the verification request.
        /// </summary>
        [JsonPropertyName("acceptLanguage")]
        public List<string> AcceptLanguage { get; set; }
    }
}
