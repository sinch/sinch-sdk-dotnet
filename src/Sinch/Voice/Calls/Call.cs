using System;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Calls
{
    public sealed class Call
    {
        /// <summary>
        ///     Contains the caller information.
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        ///     Contains the callee information.
        /// </summary>
        public string? To { get; set; }

        /// <summary>
        ///     Must be &#x60;pstn&#x60; for PSTN.
        /// </summary>
        public CallDomain? Domain { get; set; }


        /// <summary>
        ///     The unique identifier of the call.
        /// </summary>
        public string? CallId { get; set; }

        /// <summary>
        ///     The duration of the call in seconds.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        ///     The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;
        /// </summary>
        public CallStatus? Status { get; set; }


        /// <summary>
        ///     Contains the result of a call.
        /// </summary>
        public CallResult? Result { get; set; }


        /// <summary>
        ///     Contains the reason why a call ended.
        /// </summary>
        public CallResultReason? Reason { get; set; }


        /// <summary>
        ///     The date and time of the call.
        /// </summary>
        public DateTime? Timestamp { get; set; }


        /// <summary>
        ///     An object that can be used to pass custom information related to the call.
        /// </summary>
        public string? Custom { get; set; }


        /// <summary>
        ///     The rate per minute that was charged for the call.
        /// </summary>
        public string? UserRate { get; set; }


        /// <summary>
        ///     The total amount charged for the call.
        /// </summary>
        public string? Debit { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCallResponseObj {\n");
            sb.Append("  From: ").Append(From).Append("\n");
            sb.Append("  To: ").Append(To).Append("\n");
            sb.Append("  Domain: ").Append(Domain).Append("\n");
            sb.Append("  CallId: ").Append(CallId).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Custom: ").Append(Custom).Append("\n");
            sb.Append("  UserRate: ").Append(UserRate).Append("\n");
            sb.Append("  Debit: ").Append(Debit).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;
    /// </summary>
    /// <value>The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallStatus>))]
    public record CallStatus(string Value) : EnumRecord(Value)
    {
        public static readonly CallStatus Ongoing = new("ONGOING");
        public static readonly CallStatus Final = new("FINAL");
    }

    /// <summary>
    ///     Contains the result of a call.
    /// </summary>
    /// <value>Contains the result of a call.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallResult>))]
    public record CallResult(string Value) : EnumRecord(Value)
    {
        public static readonly CallResult NotAvailable = new("N/A");
        public static readonly CallResult Answered = new("ANSWERED");
        public static readonly CallResult Busy = new("BUSY");
        public static readonly CallResult NoAnswer = new("NOANSWER");
        public static readonly CallResult Failed = new("FAILED");
    }

    /// <summary>
    ///     Must be &#x60;pstn&#x60; for PSTN.
    /// </summary>
    /// <value>Must be &#x60;pstn&#x60; for PSTN.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallDomain>))]
    public record CallDomain(string Value) : EnumRecord(Value)
    {
        public static readonly CallDomain Pstn = new("pstn");
    }

    /// <summary>
    ///     Contains the reason why a call ended.
    /// </summary>
    /// <value>Contains the reason why a call ended.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallResultReason>))]
    public record CallResultReason(string Value) : EnumRecord(Value)
    {
        public static readonly CallResultReason NotAvailable = new("N/A");
        public static readonly CallResultReason Timeout = new("TIMEOUT");
        public static readonly CallResultReason CallerHangUp = new("CALLERHANGUP");
        public static readonly CallResultReason CaleeHangUp = new("CALLEEHANGUP");
        public static readonly CallResultReason Blocked = new("BLOCKED");
        public static readonly CallResultReason NoCreditPartner = new("NOCREDITPARTNER");
        public static readonly CallResultReason ManagerHangUp = new("MANAGERHANGUP");
        public static readonly CallResultReason Cancel = new("CANCEL");
        public static readonly CallResultReason GeneralError = new("GENERALERROR");
    }
}
