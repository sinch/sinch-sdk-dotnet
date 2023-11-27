using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Calls
{
    public sealed class Call
    {
        /// <summary>
        ///     Contains the caller information.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     Contains the callee information.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        ///     Must be &#x60;pstn&#x60; for PSTN.
        /// </summary>
        public DomainEnum Domain { get; set; }


        /// <summary>
        ///     The unique identifier of the call.
        /// </summary>
        public string CallId { get; set; }

        /// <summary>
        ///     The duration of the call in seconds.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        ///     The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;
        /// </summary>
        public StatusEnum Status { get; set; }


        /// <summary>
        ///     Contains the result of a call.
        /// </summary>
        public ResultEnum Result { get; set; }


        /// <summary>
        ///     Contains the reason why a call ended.
        /// </summary>
        public ReasonEnum Reason { get; set; }


        /// <summary>
        ///     The date and time of the call.
        /// </summary>
        public DateTime? Timestamp { get; set; }


        /// <summary>
        ///     An object that can be used to pass custom information related to the call.
        /// </summary>
        public object Custom { get; set; }


        /// <summary>
        ///     The rate per minute that was charged for the call.
        /// </summary>
        public string UserRate { get; set; }


        /// <summary>
        ///     The total amount charged for the call.
        /// </summary>
        public string Debit { get; set; }


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

        /// <summary>
        ///     The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;
        /// </summary>
        /// <value>The status of the call. Either &#x60;ONGOING&#x60; or &#x60;FINAL&#x60;</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<StatusEnum>))]
        public record StatusEnum(string Value) : EnumRecord(Value)
        {
            public static readonly StatusEnum ONGOING = new("ONGOING");
            public static readonly StatusEnum FINAL = new("FINAL");
        }

        /// <summary>
        ///     Contains the result of a call.
        /// </summary>
        /// <value>Contains the result of a call.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<ResultEnum>))]
        public record ResultEnum(string Value) : EnumRecord(Value)
        {
            public static readonly ResultEnum NotAvailable = new("N/A");
            public static readonly ResultEnum Answered = new("ANSWERED");
            public static readonly ResultEnum Busy = new("BUSY");
            public static readonly ResultEnum NoAnswer = new("NOANSWER");
            public static readonly ResultEnum Failed = new("FAILED");
        }
    }

    /// <summary>
    ///     Must be &#x60;pstn&#x60; for PSTN.
    /// </summary>
    /// <value>Must be &#x60;pstn&#x60; for PSTN.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<DomainEnum>))]
    public record DomainEnum(string Value) : EnumRecord(Value)
    {
        public static readonly DomainEnum Pstn = new("pstn");
    }

    /// <summary>
    ///     Contains the reason why a call ended.
    /// </summary>
    /// <value>Contains the reason why a call ended.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<ReasonEnum>))]
    public record ReasonEnum(string Value) : EnumRecord(Value)
    {
        public static readonly ReasonEnum NotAvailable = new("N/A");
        public static readonly ReasonEnum Timeout = new("TIMEOUT");
        public static readonly ReasonEnum CallerHangUp = new("CALLERHANGUP");
        public static readonly ReasonEnum CaleeHangUp = new("CALLEEHANGUP");
        public static readonly ReasonEnum Blocked = new("BLOCKED");
        public static readonly ReasonEnum NoCreditPartner = new("NOCREDITPARTNER");
        public static readonly ReasonEnum ManagerHangUp = new("MANAGERHANGUP");
        public static readonly ReasonEnum Cancel = new("CANCEL");
        public static readonly ReasonEnum GeneralError = new("GENERALERROR");
    }
}
