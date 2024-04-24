namespace Sinch.SMS.Batches.Update
{
    public class UpdateBinaryBatchRequest : UpdateBatchBaseRequest, IUpdateBatchRequest
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
    }
}
