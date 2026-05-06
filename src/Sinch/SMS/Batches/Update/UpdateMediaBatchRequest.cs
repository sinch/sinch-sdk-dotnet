using System.Collections.Generic;
using Sinch.SMS.Batches.Send;

namespace Sinch.SMS.Batches.Update
{
    public sealed class UpdateMediaBatchRequest : UpdateBatchBaseRequest, IUpdateBatchRequest
    {
        public override SmsType Type { get; } = SmsType.MtMedia;

        /// <summary>
        ///     The message content, including a URL to the media file
        /// </summary>
        public MediaBody? Body { get; set; }

        /// <summary>
        ///     Default: <c>false</c>.<br/><br/>
        ///     Whether or not you want the media included in your message to be checked against
        ///     <see href="https://developers.sinch.com/docs/mms/bestpractices/">Sinch MMS channel best practices</see>.
        ///     If set to true, your message will be rejected if it doesn't conform to the listed
        ///     recommendations, otherwise no validation will be performed.
        /// </summary>
        public bool? StrictValidation { get; set; }

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>>? Parameters { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UpdateMediaBatchRequest)} {{\n");
            sb.Append($"  {nameof(From)}: ").Append(From).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(ToAdd)}: ").Append(ToAdd).Append('\n');
            sb.Append($"  {nameof(ToRemove)}: ").Append(ToRemove).Append('\n');
            sb.Append($"  {nameof(DeliveryReport)}: ").Append(DeliveryReport).Append('\n');
            sb.Append($"  {nameof(SendAt)}: ").Append(SendAt).Append('\n');
            sb.Append($"  {nameof(ExpireAt)}: ").Append(ExpireAt).Append('\n');
            sb.Append($"  {nameof(EventDestinationTarget)}: ").Append(EventDestinationTarget).Append('\n');
            sb.Append($"  {nameof(ClientReference)}: ").Append(ClientReference).Append('\n');
            sb.Append($"  {nameof(FeedbackEnabled)}: ").Append(FeedbackEnabled).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(StrictValidation)}: ").Append(StrictValidation).Append('\n');
            sb.Append($"  {nameof(Parameters)}: ").Append(Parameters).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
