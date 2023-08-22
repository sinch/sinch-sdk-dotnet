using Sinch.Conversation.Messages.Send;
using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    public sealed class Contact : OneOf<Contact, Identified>
    {
        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string ContactId { get; set; }
    }
}
