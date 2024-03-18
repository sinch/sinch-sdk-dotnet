using System;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;

namespace Sinch.Conversation.Conversations.InjectEvent
{
    public class InjectEventRequest
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public InjectEventRequest()
        {
        }

        public InjectEventRequest(AppEvent appEvent)
        {
            AppEvent = appEvent;
        }

        public InjectEventRequest(ContactEvent contactEvent)
        {
            ContactEvent = contactEvent;
        }

        public InjectEventRequest(ContactMessageEvent contactMessageEvent)
        {
            ContactMessageEvent = contactMessageEvent;
        }

        /// <summary>
        ///     Gets or Sets AppEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AppEvent AppEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactEvent ContactEvent { get; private set; }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactMessageEvent ContactMessageEvent { get; private set; }

        /// <summary>
        ///     The processed time of the message in UTC timezone. Must be less than current_time and greater than (current_time -
        ///     30 days).
        /// </summary>
#if NET7_0_OR_GREATER
        public required DateTime AcceptTime { get; set; }
#else
        public DateTime AcceptTime { get; set; }
#endif

        /// <summary>
        ///     Optional. The ID of the contact. Will not be present for apps in Dispatch Mode.
        /// </summary>
        public string ContactId { get; set; }

        /// <summary>
        ///     A unique identity of message recipient on a particular channel. For example, the channel identity on SMS, WHATSAPP
        ///     or VIBERBM is a MSISDN phone number.
        /// </summary>
        public ChannelIdentity ChannelIdentity { get; set; }

        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app. For more information, see
        ///     [Processing Modes](https://developers.sinch.com/docs/conversation/processing-modes/).
        /// </summary>
        public ProcessingMode ProcessingMode { get; set; }
    }
}
