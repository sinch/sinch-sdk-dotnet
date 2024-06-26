namespace Sinch.SMS.Batches.Send
{
    public class SendBinaryBatchRequest : BatchBase, ISendBatchRequest
    {
        /// <summary>
        ///       The UDH header of a binary message HEX encoded. Max 140 bytes including the <c>body</c>.  
        /// </summary>
        public string? Udh { get; set; }

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

        /// <summary>
        ///     The message content Base64 encoded.<br/><br/>
        ///     Max 140 bytes including <see cref="Udh"/>.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Body { get; set; }
#else
        public string Body { get; set; } = null!;
#endif
        /// <summary>
        ///         SMS in binary format.
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtBinary;
    }
}
