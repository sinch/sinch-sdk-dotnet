using System.Collections.Generic;
using Sinch.Voice.Destinations;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.Voice.Callouts.Callout;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     When a call reaches the Sinch platform, the system makes a POST request to the specified event destination URL.
    ///     This event, called the ICE event, can be triggered by either an incoming data call or an incoming PSTN call. Look
    ///     here for allowed instructions and actions.
    ///     If there is no response to the ICE event within the timeout period, an error message is played, and the call is
    ///     disconnected.
    /// </summary>
    public sealed class IncomingCallEvent : VoiceCallSinchEvent
    {
        /// <summary>
        ///     Must have the value ice.
        /// </summary>
        [JsonPropertyName("event")]
        internal override EventType Event { get; set; } = EventType.IncomingCallEvent;


        /// <summary>
        ///     The path of the API resource.
        /// </summary>
        [JsonPropertyName("callResourceUrl")]
        public string? CallResourceUrl { get; set; }


        /// <summary>
        ///     Gets or Sets UserRate
        /// </summary>
        [JsonPropertyName("userRate")]
        public Rate? UserRate { get; set; }


        /// <summary>
        ///     The number that will be displayed to the recipient of the call. To set your own CLI, you may use your verified
        ///     number or your Dashboard virtual number and add it to the &#x60;connectPSTN&#x60; SVAML response to the Incoming
        ///     Call Event request.  It must be in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format.
        /// </summary>
        [JsonPropertyName("cli")]
        public string? Cli { get; set; }


        /// <summary>
        ///     Gets or Sets To
        /// </summary>
        [JsonPropertyName("to")]
        public ISinchEventDestination? To { get; set; }


        /// <summary>
        ///     The domain destination of the incoming call.
        /// </summary>
        [JsonPropertyName("domain")]
        public Domain? Domain { get; set; }


        /// <summary>
        ///     The origination domain of the incoming call.
        /// </summary>
        [JsonPropertyName("originationType")]
        public Domain? OriginationType { get; set; }


        /// <summary>
        ///     The duration of the call in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }


        /// <summary>
        ///     The redirected dialled number identification service.
        /// </summary>
        [JsonPropertyName("rdnis")]
        public string? Rdnis { get; set; }


        /// <summary>
        ///     If the call is initiated by a Sinch SDK client, call headers are the headers specified by the *caller* client. Read
        ///     more about call headers [here](https://developers.sinch.com/docs/voice/api-reference/call-headers).
        /// </summary>
        [JsonPropertyName("callHeaders")]
        public List<CallHeader>? CallHeaders { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IceRequest {\n");
            sb.Append("  Event: ").Append(Event).Append("\n");
            sb.Append("  CallResourceUrl: ").Append(CallResourceUrl).Append("\n");
            sb.Append("  UserRate: ").Append(UserRate).Append("\n");
            sb.Append("  Cli: ").Append(Cli).Append("\n");
            sb.Append("  To: ").Append(To).Append("\n");
            sb.Append("  Domain: ").Append(Domain).Append("\n");
            sb.Append("  OriginationType: ").Append(OriginationType).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  Rdnis: ").Append(Rdnis).Append("\n");
            sb.Append("  CallHeaders: ").Append(CallHeaders).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<OriginationType>))]
    public record OriginationType(string Value) : EnumRecord(Value)
    {
        public static readonly OriginationType Pstn = new("pstn");
        public static readonly OriginationType Mxp = new("mxp");
    }
}
