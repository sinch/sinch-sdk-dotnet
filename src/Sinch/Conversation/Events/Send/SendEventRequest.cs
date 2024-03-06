using System.Collections.Generic;
using System.Text;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Events.Send
{
    /// <summary>
    ///     SendEventRequest
    /// </summary>
    public sealed class SendEventRequest
    {
        /// <summary>
        /// Gets or Sets Queue
        /// </summary>
        public MessageQueue Queue { get; set; }

        /// <summary>
        ///     The ID of the app sending the event.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; }
#endif


        /// <summary>
        ///     Overwrites the default callback url for delivery receipts for this message The REST URL should be of the form: &#x60;http://host[:port]/path&#x60;
        /// </summary>
        public string CallbackUrl { get; set; }


        /// <summary>
        ///     Optional. A single element array that dictates on what channel should the Conversation API try to send the event. It overrides any default set on the contact. Providing more than one option has no effect.
        /// </summary>
        public List<ConversationChannel> ChannelPriorityOrder { get; set; }


        /// <summary>
        ///     Gets or Sets VarEvent
        /// </summary>
#if NET7_0_OR_GREATER
        public required AppEvent Event { get; set; }
#else
        public AppEvent Event { get; set; }
#endif


        /// <summary>
        ///     Optional. Eventual metadata that should be associated to the event.
        /// </summary>
        public string EventMetadata { get; set; }


        /// <summary>
        ///     Gets or Sets Recipient
        /// </summary>
#if NET7_0_OR_GREATER
        public required IRecipient Recipient { get; set; }
#else
        public IRecipient Recipient { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SendEventRequest {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  CallbackUrl: ").Append(CallbackUrl).Append("\n");
            sb.Append("  ChannelPriorityOrder: ").Append(ChannelPriorityOrder).Append("\n");
            sb.Append("  VarEvent: ").Append(Event).Append("\n");
            sb.Append("  EventMetadata: ").Append(EventMetadata).Append("\n");
            sb.Append("  Queue: ").Append(Queue).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
