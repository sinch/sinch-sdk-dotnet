using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment order.
    /// </summary>
    public sealed class PaymentOrderStatusChannelSpecificMessagePaymentOrder
    {
        /// <summary>
        /// The new payment message status.
        /// </summary>
        /// <value>The new payment message status.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<StatusEnum>))]
        public record StatusEnum(string Value) : EnumRecord(Value)
        {
            public static readonly StatusEnum Pending = new("pending");
            public static readonly StatusEnum Processing = new("processing");
            public static readonly StatusEnum PartiallyShipped = new("partially-shipped");
            public static readonly StatusEnum Shipped = new("shipped");
            public static readonly StatusEnum Completed = new("completed");
            public static readonly StatusEnum Canceled = new("canceled");
        }


        /// <summary>
        /// The new payment message status.
        /// </summary>
        [JsonPropertyName("status")]
#if NET7_0_OR_GREATER
        public required StatusEnum Status { get; set; }
#else
        public StatusEnum Status { get; set; } = null!;
#endif

        /// <summary>
        ///     The description of payment message status update (120 characters maximum).
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(PaymentOrderStatusChannelSpecificMessagePaymentOrder)} {{\n");
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Description)}: ").Append(Description).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
