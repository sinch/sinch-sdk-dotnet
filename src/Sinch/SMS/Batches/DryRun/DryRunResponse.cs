using System.Collections.Generic;

namespace Sinch.SMS.Batches.DryRun
{
    public class DryRunResponse
    {
        /// <summary>
        ///     The number of recipients in the batch
        /// </summary>
        public int NumberOfRecipients { get; set; }
        
        /// <summary>
        ///     The total number of SMS message parts to be sent in the batch
        /// </summary>
        public int NumberOfMessages { get; set; }
        
        /// <summary>
        ///     The recipient, the number of message parts to this recipient,
        ///     the body of the message, and the encoding type of each message
        /// </summary>
        public List<PerRecipient> PerRecipient { get; set; }
    }

    public class PerRecipient
    {
        public string Recipient { get; set; }

        public string MessagePart { get; set; }

        public string Body { get; set; }

        public string Encoding { get; set; }
    }
}
