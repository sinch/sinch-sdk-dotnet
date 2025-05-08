using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Batches.Update
{
    public sealed class UpdateBinaryBatchRequest : UpdateBatchBaseRequest, IUpdateBatchRequest
    {
        public override SmsType Type { get; } = SmsType.MtBinary;

        /// <summary>
        ///     The message content Base64 encoded.<br/><br/>
        ///     Max 140 bytes including <see cref="Udh"/>.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        ///       The UDH header of a binary message HEX encoded. Max 140 bytes including the <c>body</c>.  
        /// </summary>
        public string? Udh { get; set; }

        /// <summary>
        ///     The type of number for the sender number. Use to override the automatic detection.
        /// </summary>
        [JsonPropertyName("from_ton")]
        public int? FromTon { get; set; }


        /// <summary>
        ///     Number Plan Indicator for the sender number. Use to override the automatic detection.
        /// </summary>
        [JsonPropertyName("from_npi")]
        public int? FromNpi { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UpdateBinaryBatchRequest)} {{\n");
            sb.Append($"  {nameof(From)}: ").Append(From).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(ToAdd)}: ").Append(ToAdd).Append('\n');
            sb.Append($"  {nameof(ToRemove)}: ").Append(ToRemove).Append('\n');
            sb.Append($"  {nameof(DeliveryReport)}: ").Append(DeliveryReport).Append('\n');
            sb.Append($"  {nameof(SendAt)}: ").Append(SendAt).Append('\n');
            sb.Append($"  {nameof(ExpireAt)}: ").Append(ExpireAt).Append('\n');
            sb.Append($"  {nameof(CallbackUrl)}: ").Append(CallbackUrl).Append('\n');
            sb.Append($"  {nameof(ClientReference)}: ").Append(ClientReference).Append('\n');
            sb.Append($"  {nameof(FeedbackEnabled)}: ").Append(FeedbackEnabled).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(Udh)}: ").Append(Udh).Append('\n');
            sb.Append($"  {nameof(FromTon)}: ").Append(FromTon).Append('\n');
            sb.Append($"  {nameof(FromNpi)}: ").Append(FromNpi).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
