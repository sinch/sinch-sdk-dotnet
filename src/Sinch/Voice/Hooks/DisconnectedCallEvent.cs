using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Voice.Calls;
using Sinch.Voice.Calls.Actions;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     This callback is made when the call is disconnected. It's a POST request to the specified calling callback URL.
    ///     This event doesn't support instructions and only supports the
    ///     [hangup](https://developers.sinch.com/docs/voice/api-reference/svaml/actions/#hangup) action.
    /// </summary>
    public class DisconnectedCallEvent
    {
        /// <summary>
        ///     Must have the value &#x60;dice&#x60;.
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; }


        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callId")]
        public string CallId { get; set; }


        /// <summary>
        ///     The timestamp in UTC format.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }


        /// <summary>
        ///     The reason the call was disconnected.
        /// </summary>
        [JsonPropertyName("reason")]
        public CallResultReason Reason { get; set; }


        /// <summary>
        ///     The result of the call.
        /// </summary>
        [JsonPropertyName("result")]
        public CallResult Result { get; set; }


        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int Version { get; set; }


        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string Custom { get; set; }


        /// <summary>
        ///     Gets or Sets Debit
        /// </summary>
        [JsonPropertyName("debit")]
        public Rate Debit { get; set; }


        /// <summary>
        ///     Gets or Sets UserRate
        /// </summary>
        [JsonPropertyName("userRate")]
        public Rate UserRate { get; set; }


        /// <summary>
        ///     Gets or Sets To
        /// </summary>
        [JsonPropertyName("to")]
        public To To { get; set; }


        /// <summary>
        ///     The duration of the call in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }


        /// <summary>
        ///     Information about the initiator of the call.
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; }


        /// <summary>
        ///     If the call was initiated by a Sinch SDK client, call headers are the headers specified by the *caller* client.
        ///     Read more about call headers [here](../../../call-headers/).
        /// </summary>
        [JsonPropertyName("callHeaders")]
        public List<CallHeader> CallHeaders { get; set; }


        /// <summary>
        ///     The unique application key. You can find it in the Sinch [dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("applicationKey")]
        public string ApplicationKey { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DiceRequest {\n");
            sb.Append("  VarEvent: ").Append(Event).Append("\n");
            sb.Append("  CallId: ").Append(CallId).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  Custom: ").Append(Custom).Append("\n");
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
