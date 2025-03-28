using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Batches.Update
{
    public sealed class UpdateTextBatchRequest : UpdateBatchBaseRequest, IUpdateBatchRequest
    {
        public override SmsType Type { get; } = SmsType.MtText;

        /// <summary>
        ///     The message content
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>>? Parameters { get; set; }


        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts
        /// </summary>
        [JsonPropertyName("max_number_of_message_parts")]
        public int MaxNumberOfMessageParts { get; set; }

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
        ///     If set to true the message will be shortened when exceeding one part.
        /// </summary>
        [JsonPropertyName("truncate_concat")]
        public bool? TruncateConcat { get; set; }
        
        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        [JsonPropertyName("flash_message")]
        public bool? FlashMessage { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UpdateTextBatchRequest)} {{\n");
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
            sb.Append($"  {nameof(Parameters)}: ").Append(Parameters).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(FromTon)}: ").Append(FromTon).Append('\n');
            sb.Append($"  {nameof(FromNpi)}: ").Append(FromNpi).Append('\n');
            sb.Append($"  {nameof(MaxNumberOfMessageParts)}: ").Append(MaxNumberOfMessageParts).Append('\n');
            sb.Append($"  {nameof(TruncateConcat)}: ").Append(TruncateConcat).Append('\n');
            sb.Append($"  {nameof(FlashMessage)}: ").Append(FlashMessage).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
