using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.Voice.Callouts.Callout;
using Sinch.Voice.Calls;
using Sinch.Voice.Calls.Actions;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     When a call reaches the Sinch platform, the system makes a POST request to the specified calling callback URL.
    ///     This event, called the ICE event, can be triggered by either an incoming data call or an incoming PSTN call. Look
    ///     here for allowed instructions and actions.
    ///     If there is no response to the callback within the timeout period, an error message is played, and the call is
    ///     disconnected.
    /// </summary>
    public sealed class IncomingCallEvent : IVoiceEvent
    {
        /// <summary>
        ///     Must have the value ice.
        /// </summary>
        [JsonPropertyName("event")]
        public EventType? Event { get; set; }


        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callid")]
        public string? CallId { get; set; }


        /// <summary>
        ///     The path of the API resource.
        /// </summary>
        [JsonPropertyName("callResourceUrl")]
        public string? CallResourceUrl { get; set; }


        /// <summary>
        ///     The timestamp in UTC format.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }


        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }


        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }


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
        public To? To { get; set; }


        /// <summary>
        ///     The domain destination of the incoming call.
        /// </summary>
        [JsonPropertyName("domain")]
        public Domain? Domain { get; set; }


        /// <summary>
        ///     The unique application key. You can find it in the Sinch [dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("applicationKey")]
        public string? ApplicationKey { get; set; }


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
            sb.Append("  CallId: ").Append(CallId).Append("\n");
            sb.Append("  CallResourceUrl: ").Append(CallResourceUrl).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  Custom: ").Append(Custom).Append("\n");
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
