using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     If [Answering Machine Detection](https://developers.sinch.com/docs/voice/api-reference/amd_v2) (AMD) is enabled, this object contains information about whether the call was answered by a machine.
    /// </summary>
    public sealed class AmdObject
    {
        /// <summary>
        /// The determination by the system of who answered the call.
        /// </summary>
        /// <value>The determination by the system of who answered the call.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<StatusEnum>))]
        public record StatusEnum(string Value) : EnumRecord(Value)
        {
            public static readonly StatusEnum Machine = new("machine");
            public static readonly StatusEnum Human = new("human");
            public static readonly StatusEnum NotSure = new("notsure");
            public static readonly StatusEnum Hangup = new("hangup");
        }


        /// <summary>
        /// The determination by the system of who answered the call.
        /// </summary>
        [JsonPropertyName("status")]
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// The reason that the system used to determine who answered the call.
        /// </summary>
        /// <value>The reason that the system used to determine who answered the call.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<ReasonEnum>))]
        public record ReasonEnum(string Value) : EnumRecord(Value)
        {
            public static readonly ReasonEnum LongGreeting = new("longgreeting");
            public static readonly ReasonEnum InitialSilence = new("initialsilence");
        }


        /// <summary>
        /// The reason that the system used to determine who answered the call.
        /// </summary>
        [JsonPropertyName("reason")]
        public ReasonEnum? Reason { get; set; }

        /// <summary>
        ///     The length of the call.
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(AmdObject)} {{\n");
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Reason)}: ").Append(Reason).Append('\n');
            sb.Append($"  {nameof(Duration)}: ").Append(Duration).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
