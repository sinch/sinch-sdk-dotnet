using System.Collections.Generic;

namespace Sinch.SMS.Batches.Send
{
    public class SendTextBatchRequest : BatchBase, ISendBatchRequest
    {
        /// <summary>
        ///     The message content
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Body { get; set; }

#else
        public string Body { get; set; } = null!;
#endif

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>>? Parameters { get; set; }

        /// <summary>
        ///     Regular SMS
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtText;

        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        public bool? FlashMessage { get; set; }

        /// <summary>
        ///     If set to true the message will be shortened when exceeding one part.
        /// </summary>
        public bool? TruncateConcat { get; set; }

        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts
        /// </summary>
        public int? MaxNumberOfMessageParts { get; set; }

        /// <summary>
        ///     The type of number for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromTon { get; set; }

        /// <summary>
        ///     Number Plan Indicator for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromNpi { get; set; }
    }
}
