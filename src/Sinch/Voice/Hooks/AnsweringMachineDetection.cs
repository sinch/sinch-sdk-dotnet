using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     If [Answering Machine Detection](https://developers.sinch.com/docs/voice/api-reference/amd_v2) (AMD) is enabled, this object contains information about whether the call was answered by a machine.
    /// </summary>
    // name ref: AmdObject
    public sealed class AnsweringMachineDetection
    {
        /// <summary>
        /// The determination by the system of who answered the call.
        /// </summary>
        /// <value>The determination by the system of who answered the call.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<AnsweringMachineDetectionStatus>))]
        public record AnsweringMachineDetectionStatus(string Value) : EnumRecord(Value)
        {
            public static readonly AnsweringMachineDetectionStatus Machine = new("machine");
            public static readonly AnsweringMachineDetectionStatus Human = new("human");
            public static readonly AnsweringMachineDetectionStatus NotSure = new("notsure");
            public static readonly AnsweringMachineDetectionStatus Hangup = new("hangup");
        }


        /// <summary>
        /// The determination by the system of who answered the call.
        /// </summary>
        [JsonPropertyName("status")]
        public AnsweringMachineDetectionStatus? Status { get; set; }

        /// <summary>
        /// The reason that the system used to determine who answered the call.
        /// </summary>
        /// <value>The reason that the system used to determine who answered the call.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<AnsweringMachineDetectionReason>))]
        public record AnsweringMachineDetectionReason(string Value) : EnumRecord(Value)
        {
            public static readonly AnsweringMachineDetectionReason LongGreeting = new("longgreeting");
            public static readonly AnsweringMachineDetectionReason InitialSilence = new("initialsilence");
        }


        /// <summary>
        /// The reason that the system used to determine who answered the call.
        /// </summary>
        [JsonPropertyName("reason")]
        public AnsweringMachineDetectionReason? Reason { get; set; }

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
            sb.Append($"class {nameof(AnsweringMachineDetection)} {{\n");
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Reason)}: ").Append(Reason).Append('\n');
            sb.Append($"  {nameof(Duration)}: ").Append(Duration).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
