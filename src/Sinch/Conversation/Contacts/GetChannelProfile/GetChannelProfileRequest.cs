using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Contacts.GetChannelProfile
{
    public class GetChannelProfileRequest
    {
        /// <summary>
        ///     The recipient to check profile information. Requires either contact_id or identified_by.
        /// </summary>
        /// <returns></returns>
#if NET7_0_OR_GREATER
        public required IRecipient Recipient { get; set; }
#else
        public IRecipient  Recipient { get; set; }
#endif

        /// <summary>
        ///     The ID of the app.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; }
#endif

        /// <summary>
        ///     The channel. Must be one of the supported channels for this operation.
        /// </summary>
#if NET7_0_OR_GREATER
        public required ChannelProfileConversationChannel Channel { get; set; }
#else
        public ChannelProfileConversationChannel Channel { get; set; }
#endif
    }
}
