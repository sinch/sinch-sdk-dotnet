using System.Text.Json.Serialization;

namespace Sinch.Verification.Hooks
{
    public sealed class CalloutRequestEventResponse : RequestEventResponseBase
    {
        [JsonPropertyName("callout")]
        public Callout? Callout { get; set; }
    }

    public sealed class Callout
    {
        /// <summary>
        ///     The Phone Call PIN that should be entered by the user.
        ///     Sinch servers automatically generate PIN codes for Phone Call verification.
        ///     If you want to set your own code, you can specify it in the response to the Verification Request Event.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        ///     An object defining various properties for the text-to-speech message.
        /// </summary>
        [JsonPropertyName("speech")]
        public Speech? Speech { get; set; }
    }

    public sealed class Speech
    {
        /// <summary>
        ///     Indicates the language that should be used for the text-to-speech message.
        ///     Currently, only en-US is supported.
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; set; }
    }
}
