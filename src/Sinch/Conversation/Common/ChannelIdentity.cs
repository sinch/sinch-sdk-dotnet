namespace Sinch.Conversation.Common
{
    public class ChannelIdentity
    {
        /// <summary>
        ///     Required if using a channel that uses app-scoped channel identities. Currently, FB Messenger, Viber Bot, Instagram,
        ///     Apple Messages for Business, LINE, and WeChat use app-scoped channel identities, which means contacts will have
        ///     different channel identities on different Conversation API apps. These can be thought of as virtual identities that
        ///     are app-specific and, therefore, the app_id must be included in the API call.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        ///     The channel identity. This will differ from channel to channel. For example, a phone number for SMS, WhatsApp, and
        ///     Viber Business.
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        ///     The identifier of the channel you want to include. Must be one of the enum values. See
        ///     <see cref="ConversationChannel" /> fields.
        /// </summary>
        public ConversationChannel Channel { get; set; }
    }
}
