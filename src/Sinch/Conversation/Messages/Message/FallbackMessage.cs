using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Fallback message. Used when original contact message can not be handled.
    /// </summary>
    public sealed class FallbackMessage
    {
        /// <summary>
        ///     Optional. The raw fallback message if provided by the channel.
        /// </summary>
        [JsonPropertyName("raw_message")]
        public string? RawMessage { get; set; }


        /// <summary>
        ///     Gets or Sets Reason
        /// </summary>
        [JsonPropertyName("reason")]
        public Reason? Reason { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FallbackMessage {\n");
            sb.Append("  RawMessage: ").Append(RawMessage).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Reason
    /// </summary>
    public sealed class Reason
    {
        /// <summary>
        ///     Gets or Sets Code
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }


        /// <summary>
        ///     A textual description of the reason.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Gets or Sets SubCode
        /// </summary>
        [JsonPropertyName("sub_code")]
        public string? SubCode { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Reason {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  SubCode: ").Append(SubCode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
