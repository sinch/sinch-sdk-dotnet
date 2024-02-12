using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Webhooks
{
    /// <summary>
    ///     - &#x60;UNSPECIFIED_TRIGGER&#x60;: Using this value will cause errors. - &#x60;MESSAGE_DELIVERY&#x60;: Subscribe to
    ///     delivery receipts for a message sent. - &#x60;MESSAGE_SUBMIT&#x60;: Subscribe to message submission notifications.
    ///     - &#x60;EVENT_DELIVERY&#x60;: Subscribe to delivery receipts for a event sent. - &#x60;MESSAGE_INBOUND&#x60;:
    ///     Subscribe to inbound messages from end users on the underlying channels. - &#x60;SMART_CONVERSATION&#x60;: These
    ///     triggers allow you to subscribe to payloads that provide machine learning analyses of inbound messages from end
    ///     users on the underlying channels  - &#x60;MESSAGE_INBOUND_SMART_CONVERSATION_REDACTION&#x60;: These triggers allow
    ///     you to subscribe to payloads that deliver redacted versions of inbound messages  - &#x60;EVENT_INBOUND&#x60;:
    ///     Subscribe to inbound events from end users on the underlying channels. - &#x60;CONVERSATION_START&#x60;: Subscribe
    ///     to an event that is triggered when a new conversation has been started. - &#x60;CONVERSATION_STOP&#x60;: Subscribe
    ///     to an event that is triggered when an active conversation has been stopped. - &#x60;CONTACT_CREATE&#x60;: Subscribe
    ///     to an event that is triggered when a new contact has been created. - &#x60;CONTACT_DELETE&#x60;: Subscribe to an
    ///     event that is triggered when a contact has been deleted. - &#x60;CONTACT_MERGE&#x60;: Subscribe to an event that is
    ///     triggered when two contacts are merged. - &#x60;CONTACT_UPDATE&#x60;: Subscribe to an event that is triggered when
    ///     a contact is updated. - &#x60;UNSUPPORTED&#x60;: Subscribe to callbacks that are not natively supported by the
    ///     Conversation API. - &#x60;OPT_IN&#x60;: Subscribe to opt_ins. - &#x60;OPT_OUT&#x60;: Subscribe to opt_outs. -
    ///     &#x60;CAPABILITY&#x60;: Subscribe to see get capability results. - &#x60;CHANNEL_EVENT&#x60;: Subscribe to channel
    ///     event notifications. - &#x60;CONVERSATION_DELETE&#x60;: Subscribe to get an event when a conversation is deleted. -
    ///     &#x60;CONTACT_IDENTITIES_DUPLICATION&#x60;: Subscribe to get an event when contact identity duplications are found
    ///     during message or event processing. - &#x60;SMART_CONVERSATIONS&#x60;: Subscribe to smart conversations callback
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<WebhookTrigger>))]
    public record WebhookTrigger(string Value) : EnumRecord(Value)
    {
        public static readonly WebhookTrigger UnspecifiedTrigger = new("UNSPECIFIED_TRIGGER");
        public static readonly WebhookTrigger MessageDelivery = new("MESSAGE_DELIVERY");
        public static readonly WebhookTrigger MessageSubmit = new("MESSAGE_SUBMIT");
        public static readonly WebhookTrigger EventDelivery = new("EVENT_DELIVERY");
        public static readonly WebhookTrigger MessageInbound = new("MESSAGE_INBOUND");
        public static readonly WebhookTrigger SmartConversation = new("SMART_CONVERSATION");

        public static readonly WebhookTrigger MessageInboundSmartConversationRedaction =
            new("MESSAGE_INBOUND_SMART_CONVERSATION_REDACTION");

        public static readonly WebhookTrigger EventInbound = new("EVENT_INBOUND");
        public static readonly WebhookTrigger ConversationStart = new("CONVERSATION_START");
        public static readonly WebhookTrigger ConversationStop = new("CONVERSATION_STOP");
        public static readonly WebhookTrigger ContactCreate = new("CONTACT_CREATE");
        public static readonly WebhookTrigger ContactDelete = new("CONTACT_DELETE");
        public static readonly WebhookTrigger ContactMerge = new("CONTACT_MERGE");
        public static readonly WebhookTrigger ContactUpdate = new("CONTACT_UPDATE");
        public static readonly WebhookTrigger Unsupported = new("UNSUPPORTED");
        public static readonly WebhookTrigger OptIn = new("OPT_IN");
        public static readonly WebhookTrigger OptOut = new("OPT_OUT");
        public static readonly WebhookTrigger Capability = new("CAPABILITY");
        public static readonly WebhookTrigger ChannelEvent = new("CHANNEL_EVENT");
        public static readonly WebhookTrigger ConversationDelete = new("CONVERSATION_DELETE");
        public static readonly WebhookTrigger ContactIdentitiesDuplication = new("CONTACT_IDENTITIES_DUPLICATION");
    }
}
