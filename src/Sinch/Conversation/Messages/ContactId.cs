namespace Sinch.Conversation.Messages
{
    public sealed class Contact : IRecipient
    {
        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string ContactId { get; set; }
    }
}
