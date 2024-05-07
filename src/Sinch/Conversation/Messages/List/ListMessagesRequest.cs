using System;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.List
{
    public class ListMessagesRequest
    {
        /// <summary>
        ///     Id of the conversation. Available only when messages_source is CONVERSATION_SOURCE.
        /// </summary>
        public string? ConversationId { get; set; }

        /// <summary>
        ///     Id of the contact. Available only when messages_source is CONVERSATION_SOURCE.
        /// </summary>
        public string? ContactId { get; set; }

        /// <summary>
        ///     Id of the app.
        /// </summary>
        public string? AppId { get; set; }

        /// <summary>
        ///     Channel identity of the contact.
        /// </summary>
        public string? ChannelIdentity { get; set; }

        /// <summary>
        ///     Filter messages with accept_time after this timestamp. Must be before end_time if that is specified.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        ///     Filter messages with accept_time before this timestamp.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///     Maximum number of messages to fetch. Defaults to 10 and the maximum is 1000.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Next page token previously returned if any. When specifying this token,
        ///     make sure to use the same values for the other parameters from the request that originated the token,
        ///     otherwise the paged results may be inconsistent.
        /// </summary>
        public string? PageToken { get; set; }

        public View? View { get; set; }

        /// <summary>
        ///     Specifies the message source for which the request will be processed.
        ///     Used for operations on messages in Dispatch Mode. For more information, see Processing Modes.
        /// </summary>
        public MessageSource? MessageSource { get; set; }

        /// <summary>
        ///     If true, fetch only recipient originated messages.
        ///     Available only when messages_source is DISPATCH_SOURCE.
        /// </summary>
        public bool? OnlyRecipientOriginated { get; set; }
    }


    /// <summary>
    /// Represents the view options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<View>))]
    public record View(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// View with metadata.
        /// </summary>
        public static readonly View WithMetadata = new("WITH_METADATA");

        /// <summary>
        /// View without metadata.
        /// </summary>
        public static readonly View WithoutMetadata = new("WITHOUT_METADATA");
    }
}
