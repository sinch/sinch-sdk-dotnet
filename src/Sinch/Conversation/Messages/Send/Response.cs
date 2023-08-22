using System;

namespace Sinch.Conversation.Messages.Send
{
    public class Response
    {
        /// <summary>
        ///     Timestamp when the Conversation API accepted the message for delivery to the referenced contact.
        /// </summary>
        public DateTime AcceptedTime { get; set; }
        
        /// <summary>
        ///     The ID of the message.
        /// </summary>
        public string MessageId { get; set; }
        
    }
}
