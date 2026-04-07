namespace Sinch.Conversation.Messages.Update
{
    public sealed class UpdateMessageRequest
    {
        /// <summary>
        ///     Metadata associated with the message. Up to 1024 characters long.
        /// </summary>
        public required string Metadata { get; set; }
    }
}
