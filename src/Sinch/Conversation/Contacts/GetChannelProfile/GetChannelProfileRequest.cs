using Sinch.Conversation.Common;

namespace Sinch.Conversation.Contacts.GetChannelProfile
{
    public class GetChannelProfileRequest
    {
        /// <summary>
        ///     The recipient to check profile information. Requires either contact_id or identified_by.
        /// </summary>
        /// <returns></returns>

        public required IRecipient Recipient { get; set; }


        /// <summary>
        ///     The ID of the app.
        /// </summary>

        public required string AppId { get; set; }


        /// <summary>
        ///     The channel. Must be one of the supported channels for this operation.
        /// </summary>

        public required ChannelProfileConversationChannel Channel { get; set; }

    }
}
