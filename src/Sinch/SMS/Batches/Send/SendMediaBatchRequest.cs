using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Batches.Send
{
    /// <summary>
    ///    Only available in the US. Contact support if you wish to send MMS.
    /// </summary>
    public sealed class SendMediaBatchRequest : BatchBase, ISendBatchRequest
    {
        /// <summary>
        ///     The message content, including a URL to the media file
        /// </summary>

        public required MediaBody Body { get; set; }

        /// <summary>
        ///     MMS
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtMedia;

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
    }

    /// <summary>
    ///     The message content, including a URL to the media file
    /// </summary>
    public sealed class MediaBody
    {
        /// <summary>
        ///     The subject text
        /// </summary>
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }


        /// <summary>
        ///     The message text. Text only media messages will be rejected, please use SMS instead.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }


        /// <summary>
        ///     URL to the media file
        /// </summary>
        [JsonPropertyName("url")]
        public required Uri Url { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MediaBody)} {{\n");
            sb.Append($"  {nameof(Subject)}: ").Append(Subject).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
