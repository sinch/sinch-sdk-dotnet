using System;
using System.Collections.Generic;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Messages.List
{
    /// <summary>
    ///     Request body for listing messages by channel identity.
    /// </summary>
    public sealed class ListMessagesByChannelIdentityRequest
    {
        /// <summary>
        ///     Filter messages by <c>channel_identity</c>.
        /// </summary>
        public List<string>? ChannelIdentities { get; set; }

        /// <summary>
        ///     Resource name (id) of the contact.
        ///     In <c>CONVERSATION_SOURCE</c> mode: can list last messages by contact_id.
        ///     In <c>DISPATCH_SOURCE</c> mode: unsupported.
        /// </summary>
        public List<string>? ContactIds { get; set; }

        /// <summary>
        ///     Resource name (id) of the app.
        /// </summary>
        public string? AppId { get; set; }

        /// <summary>
        ///     Specifies the message source for which the request will be processed.
        ///     Default is <c>DISPATCH_SOURCE</c>.
        /// </summary>
        public MessageSource? MessagesSource { get; set; }

        /// <summary>
        ///     Optional. Maximum number of messages to fetch. Defaults to 10 and the maximum is 1000.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Next page token previously returned if any.
        /// </summary>
        public string? PageToken { get; set; }

        /// <summary>
        ///     Specifies the representation in which messages should be returned.
        ///     Default to <c>WITH_METADATA</c>.
        /// </summary>
        public View? View { get; set; }

        /// <summary>
        ///     Only fetch messages with <c>accept_time</c> after this date.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        ///     Only fetch messages with <c>accept_time</c> before this date.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///     Only fetch messages from the specified channel.
        /// </summary>
        public ConversationChannel? Channel { get; set; }

        /// <summary>
        ///     Only fetch messages with the specified direction.
        ///     If direction is not specified, it will list both TO_APP and TO_CONTACT messages.
        /// </summary>
        public ConversationDirection? Direction { get; set; }
    }
}
