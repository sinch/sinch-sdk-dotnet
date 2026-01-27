using System.Text.Json.Serialization;
using Sinch.Conversation.Events.ContactEvents;
using Sinch.Conversation.Events.EventTypes;

namespace Sinch.Conversation.Events
{
    /// <summary>
    ///     The content of the events.
    /// </summary>
    public sealed class ContactEvent
    {
        [JsonConstructor]
        private ContactEvent() { }

        public ContactEvent(ComposingEvent composingEvent) => ComposingEvent = composingEvent;

        public ContactEvent(ComposingEndEvent composingEndEvent) => ComposingEndEvent = composingEndEvent;

        public ContactEvent(ConversationDeletedEvent conversationDeletedEvent) =>
            ConversationDeletedEvent = conversationDeletedEvent;

        public ContactEvent(CommentEvent commentEvent) => CommentEvent = commentEvent;

        public ContactEvent(GenericEvent genericEvent) => GenericEvent = genericEvent;

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEvent? ComposingEvent { get; init; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEndEvent? ComposingEndEvent { get; init; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConversationDeletedEvent? ConversationDeletedEvent { get; init; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CommentEvent? CommentEvent { get; init; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GenericEvent? GenericEvent { get; init; }
    }
}
