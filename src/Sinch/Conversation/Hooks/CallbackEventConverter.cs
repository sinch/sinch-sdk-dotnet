using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    /// JSON converter for ICallbackEvent that uses discriminator properties to determine the concrete event type.
    /// Each callback event type has a unique property (e.g., "message" for MessageInboundEvent,
    /// "capability_notification" for CapabilityEvent) that identifies it.
    /// </summary>
    public sealed class CallbackEventConverter : JsonConverter<ICallbackEvent>
    {
        public override ICallbackEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);

            if (elem.TryGetProperty("message", out _))
            {
                return elem.Deserialize<MessageInboundEvent>(options);
            }

            if (elem.TryGetProperty("message_redaction", out _))
            {
                return elem.Deserialize<MessageInboundSmartConversationRedactionEvent>(options);
            }

            if (elem.TryGetProperty("message_delivery_report", out _))
            {
                return elem.Deserialize<MessageDeliveryReceiptEvent>(options);
            }

            if (elem.TryGetProperty("message_submit_notification", out _))
            {
                return elem.Deserialize<MessageSubmitEvent>(options);
            }

            if (elem.TryGetProperty("event_delivery_report", out _))
            {
                return elem.Deserialize<DeliveryEvent>(options);
            }

            if (elem.TryGetProperty("event", out _))
            {
                return elem.Deserialize<InboundEvent>(options);
            }

            if (elem.TryGetProperty("capability_notification", out _))
            {
                return elem.Deserialize<CapabilityEvent>(options);
            }

            if (elem.TryGetProperty("channel_event_notification", out _))
            {
                return elem.Deserialize<ChannelEvent>(options);
            }

            if (elem.TryGetProperty("contact_create_notification", out _))
            {
                return elem.Deserialize<ContactCreateEvent>(options);
            }

            if (elem.TryGetProperty("contact_delete_notification", out _))
            {
                return elem.Deserialize<ContactDeleteEvent>(options);
            }

            if (elem.TryGetProperty("contact_update_notification", out _))
            {
                return elem.Deserialize<ContactUpdateEvent>(options);
            }

            if (elem.TryGetProperty("contact_merge_notification", out _))
            {
                return elem.Deserialize<ContactMergeEvent>(options);
            }

            if (elem.TryGetProperty("duplicated_contact_identities_notification", out _))
            {
                return elem.Deserialize<ContactIdentitiesDuplicationEvent>(options);
            }

            if (elem.TryGetProperty("conversation_start_notification", out _))
            {
                return elem.Deserialize<ConversationStartEvent>(options);
            }

            if (elem.TryGetProperty("conversation_stop_notification", out _))
            {
                return elem.Deserialize<ConversationStopEvent>(options);
            }

            if (elem.TryGetProperty("conversation_delete_notification", out _))
            {
                return elem.Deserialize<ConversationDeleteEvent>(options);
            }

            if (elem.TryGetProperty("smart_conversation_notification", out _))
            {
                return elem.Deserialize<SmartConversationsEvent>(options);
            }

            if (elem.TryGetProperty("opt_in_notification", out _))
            {
                return elem.Deserialize<OptInEvent>(options);
            }

            if (elem.TryGetProperty("opt_out_notification", out _))
            {
                return elem.Deserialize<OptOutEvent>(options);
            }

            if (elem.TryGetProperty("unsupported_callback", out _))
            {
                return elem.Deserialize<UnsupportedCallbackEvent>(options);
            }

            // No matching event type found
            return null;
        }

        public override void Write(Utf8JsonWriter writer, ICallbackEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case MessageInboundEvent messageInboundEvent:
                    JsonSerializer.Serialize(writer, messageInboundEvent, options);
                    break;
                case MessageInboundSmartConversationRedactionEvent redactionEvent:
                    JsonSerializer.Serialize(writer, redactionEvent, options);
                    break;
                case MessageDeliveryReceiptEvent deliveryReceiptEvent:
                    JsonSerializer.Serialize(writer, deliveryReceiptEvent, options);
                    break;
                case MessageSubmitEvent submitEvent:
                    JsonSerializer.Serialize(writer, submitEvent, options);
                    break;
                case DeliveryEvent deliveryEvent:
                    JsonSerializer.Serialize(writer, deliveryEvent, options);
                    break;
                case InboundEvent inboundEvent:
                    JsonSerializer.Serialize(writer, inboundEvent, options);
                    break;
                case CapabilityEvent capabilityEvent:
                    JsonSerializer.Serialize(writer, capabilityEvent, options);
                    break;
                case ChannelEvent channelEvent:
                    JsonSerializer.Serialize(writer, channelEvent, options);
                    break;
                case ContactCreateEvent contactCreateEvent:
                    JsonSerializer.Serialize(writer, contactCreateEvent, options);
                    break;
                case ContactDeleteEvent contactDeleteEvent:
                    JsonSerializer.Serialize(writer, contactDeleteEvent, options);
                    break;
                case ContactUpdateEvent contactUpdateEvent:
                    JsonSerializer.Serialize(writer, contactUpdateEvent, options);
                    break;
                case ContactMergeEvent contactMergeEvent:
                    JsonSerializer.Serialize(writer, contactMergeEvent, options);
                    break;
                case ContactIdentitiesDuplicationEvent duplicationEvent:
                    JsonSerializer.Serialize(writer, duplicationEvent, options);
                    break;
                case ConversationStartEvent startEvent:
                    JsonSerializer.Serialize(writer, startEvent, options);
                    break;
                case ConversationStopEvent stopEvent:
                    JsonSerializer.Serialize(writer, stopEvent, options);
                    break;
                case ConversationDeleteEvent deleteEvent:
                    JsonSerializer.Serialize(writer, deleteEvent, options);
                    break;
                case SmartConversationsEvent smartEvent:
                    JsonSerializer.Serialize(writer, smartEvent, options);
                    break;
                case OptInEvent optInEvent:
                    JsonSerializer.Serialize(writer, optInEvent, options);
                    break;
                case OptOutEvent optOutEvent:
                    JsonSerializer.Serialize(writer, optOutEvent, options);
                    break;
                case UnsupportedCallbackEvent unsupportedEvent:
                    JsonSerializer.Serialize(writer, unsupportedEvent, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a matching class for the interface {nameof(ICallbackEvent)}");
            }
        }
    }
}
