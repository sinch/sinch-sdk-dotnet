namespace Sinch.Conversation.Messages.Message
{
    public class Choice
    {
        /// <summary>
        ///     Message for triggering a call.
        /// </summary>
        public CallMessage? CallMessage { get; set; }

        /// <summary>
        ///     Message containing geographic location.
        /// </summary>
        public LocationMessage? LocationMessage { get; set; }

        /// <summary>
        ///     An optional field. This data will be returned in the ChoiceResponseMessage.
        ///     The default is message_id_{text, title}.
        /// </summary>
        public string? PostbackData { get; set; }

        /// <summary>
        ///     A message containing only text.
        /// </summary>
        public TextMessage? TextMessage { get; set; }

        /// <summary>
        ///     A generic URL message.
        /// </summary>
        public UrlMessage? UrlMessage { get; set; }
    }
}
