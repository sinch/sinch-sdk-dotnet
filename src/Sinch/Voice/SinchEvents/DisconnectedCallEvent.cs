using System.Collections.Generic;
using Sinch.Voice.Destinations;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Voice.Calls;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     This Sinch event is sent when the call is disconnected. It's a POST request to the specified event destination URL.
    ///     This event doesn't support instructions and only supports the
    ///     [hangup](https://developers.sinch.com/docs/voice/api-reference/svaml/actions/#hangup) action.
    /// </summary>
    public sealed class DisconnectedCallEvent : VoiceCallSinchEvent
    {
        /// <summary>
        ///     Must have the value &#x60;dice&#x60;.
        /// </summary>
        [JsonPropertyName("event")]
        internal override EventType Event { get; set; } = EventType.DisconnectedCallEvent;


        /// <summary>
        ///     The reason the call was disconnected.
        /// </summary>
        [JsonPropertyName("reason")]
        public CallResultReason? Reason { get; set; }


        /// <summary>
        ///     The result of the call.
        /// </summary>
        [JsonPropertyName("result")]
        public CallResult? Result { get; set; }


        /// <summary>
        ///     Gets or Sets Debit
        /// </summary>
        [JsonPropertyName("debit")]
        public Rate? Debit { get; set; }


        /// <summary>
        ///     Gets or Sets UserRate
        /// </summary>
        [JsonPropertyName("userRate")]
        public Rate? UserRate { get; set; }


        /// <summary>
        ///     Gets or Sets To
        /// </summary>
        [JsonPropertyName("to")]
        public ISinchEventDestination? To { get; set; }


        /// <summary>
        ///     The duration of the call in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }


        /// <summary>
        ///     Information about the initiator of the call.
        /// </summary>
        [JsonPropertyName("from")]
        public string? From { get; set; }


        /// <summary>
        ///     If the call was initiated by a Sinch SDK client, call headers are the headers specified by the *caller* client.
        ///     Read more about call headers [here](../../../call-headers/).
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
            sb.Append("class DiceRequest {\n");
            sb.Append("  Event: ").Append(Event).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Debit: ").Append(Debit).Append("\n");
            sb.Append("  UserRate: ").Append(UserRate).Append("\n");
            sb.Append("  To: ").Append(To).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  From: ").Append(From).Append("\n");
            sb.Append("  CallHeaders: ").Append(CallHeaders).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
