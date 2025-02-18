using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Events
{
    /// <summary>
    ///     An event on a particular channel.
    /// </summary>
    public sealed class ConversationEvent
    {
        /// <summary>
        /// Gets or Sets Direction
        /// </summary>
        public ConversationDirection? Direction { get; set; }

        /// <summary>
        ///     Gets or Sets VarEvent
        /// </summary>
        public ConversationEventEvent? Event { get; set; }


        /// <summary>
        ///     The ID of the event.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Id { get; set; }
#else
        public string Id { get; set; } = null!;
#endif


        /// <summary>
        ///     Optional. The ID of the event&#39;s conversation. Will not be present for apps in Dispatch Mode.
        /// </summary>
        public string? ConversationId { get; set; }


        /// <summary>
        ///     Optional. The ID of the contact. Will not be present for apps in Dispatch Mode.
        /// </summary>
        public string? ContactId { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
#if NET7_0_OR_GREATER
        public required ChannelIdentity ChannelIdentity { get; set; }
#else
        public ChannelIdentity ChannelIdentity { get; set; } = null!;
#endif

        /// <summary>
        ///     Gets or Sets AcceptTime
        /// </summary>
        public DateTime AcceptTime { get; set; }


        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app. For more information, see [Processing Modes](../../../../../conversation/processing-modes/).
        /// </summary>
#if NET7_0_OR_GREATER
        public required ProcessingMode ProcessingMode { get; set; }
#else
        public ProcessingMode ProcessingMode { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConversationEvent {\n");
            sb.Append("  Direction: ").Append(Direction).Append("\n");
            sb.Append("  Event: ").Append(Event).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ConversationId: ").Append(ConversationId).Append("\n");
            sb.Append("  ContactId: ").Append(ContactId).Append("\n");
            sb.Append("  ChannelIdentity: ").Append(ChannelIdentity).Append("\n");
            sb.Append("  AcceptTime: ").Append(AcceptTime).Append("\n");
            sb.Append("  ProcessingMode: ").Append(ProcessingMode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public sealed class ConversationEventEvent
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public ConversationEventEvent()
        {
        }

        public ConversationEventEvent(AppEvent appEvent)
        {
            AppEvent = appEvent;
        }

        public ConversationEventEvent(ContactEvent contactEvent)
        {
            ContactEvent = contactEvent;
        }

        public ConversationEventEvent(ContactMessageEvent contactMessageEvent)
        {
            ContactMessageEvent = contactMessageEvent;
        }

        /// <summary>
        ///     Gets or Sets AppEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AppEvent? AppEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactEvent? ContactEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactMessageEvent? ContactMessageEvent { get; private set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConversationEventEvent {\n");
            sb.Append("  AppEvent: ").Append(AppEvent).Append("\n");
            sb.Append("  ContactEvent: ").Append(ContactEvent).Append("\n");
            sb.Append("  ContactMessageEvent: ").Append(ContactMessageEvent).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
