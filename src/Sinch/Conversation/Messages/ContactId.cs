namespace Sinch.Conversation.Messages
{
    public sealed class ContactRecipient : IRecipient
    {
        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string ContactId { get; set; }
    }
}
