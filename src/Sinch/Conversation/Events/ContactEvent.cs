using System;
using System.Text.Json.Serialization;
using Sinch.Conversation.Events.ContactEvents;
using Sinch.Conversation.Events.EventTypes;

namespace Sinch.Conversation.Events
{
    /// <summary>
    ///     The content of the events.
    /// </summary>
    public class ContactEvent
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public ContactEvent()
        {
        }

        public ContactEvent(ComposingEvent composingEvent)
        {
            ComposingEvent = composingEvent;
        }

        public ContactEvent(ComposingEndEvent composingEndEvent)
        {
            ComposingEndEvent = composingEndEvent;
        }

        public ContactEvent(ConversationDeletedEvent conversationDeletedEvent)
        {
            ConversationDeletedEvent = conversationDeletedEvent;
        }

        public ContactEvent(CommentEvent commentEvent)
        {
            CommentEvent = commentEvent;
        }

        public ContactEvent(GenericEvent genericEvent)
        {
            GenericEvent = genericEvent;
        }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEvent? ComposingEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEndEvent? ComposingEndEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ConversationDeletedEvent? ConversationDeletedEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CommentEvent? CommentEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GenericEvent? GenericEvent { get; private set; }
    }
}
