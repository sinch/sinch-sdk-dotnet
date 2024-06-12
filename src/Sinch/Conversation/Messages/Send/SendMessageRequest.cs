using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Messages.Send
{
    /// <summary>
    ///     This is the request body for sending a message. `app_id`, `recipient`, and `message` are all required fields.
    /// </summary>
    public sealed class SendMessageRequest
    {
        /// <summary>
        ///     The ID of the app sending the message.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; } = null!;
#endif

        /// <summary>
        ///     Select the priority type for the message
        /// </summary>
        public MessageQueue? Queue { get; set; }


        /// <summary>
        ///     Overwrites the default callback url for delivery receipts for this message The REST URL should be of the form: &#x60;http://host[:port]/path&#x60;
        /// </summary>
        public Uri? CallbackUrl { get; set; }


        /// <summary>
        ///     Explicitly define the channels and order in which they are tried when sending the message.
        ///     All channels provided in this field must be configured in the corresponding Conversation API app,
        ///     or the request will be rejected. Which channels the API will try and their priority is defined by:<br/>
        ///     <list type="number">
        ///             <item>`channel_priority_order` if available.</item>
        ///             <item>`recipient.identified_by.channel_identities` if available.</item>
        ///             <item>
        ///                     When recipient is a &#x60;contact_id&#x60;:
        ///                     <list type="bullet">
        ///                             <item>
        ///                             if a conversation with the contact exists: the active channel of
        ///                             the conversation is tried first.
        ///                             </item>
        ///                             <item>
        ///                             the existing channels for the contact are ordered by contact channel
        ///                             preferences if given.
        ///                             </item>
        ///                             <item>lastly the existing channels for the contact are ordered
        ///                             by the app priority.</item>
        ///                     </list>
        ///             </item>
        ///     </list>
        /// </summary>
        public List<ConversationChannel>? ChannelPriorityOrder { get; set; }


        /// <summary>
        ///     Channel-specific properties.
        ///     The key in the map must point to a valid channel property key as defined
        ///     by the enum ChannelPropertyKeys. The maximum allowed property value length is 1024 characters.
        /// </summary>
        public Dictionary<string, string>? ChannelProperties { get; set; }


#if NET7_0_OR_GREATER
        public required AppMessage Message { get; set; }
#else
        public AppMessage Message { get; set; } = null!;
#endif


        /// <summary>
        ///     Metadata that should be associated with the message. Returned in the `metadata` field of a
        ///     [Message Delivery Receipt](https://developers.sinch.com/docs/conversation/callbacks/#message-delivery-receipt).
        ///     Up to 1024 characters long.
        /// </summary>
        public string? MessageMetadata { get; set; }


        /// <summary>
        ///     Metadata that should be associated with the conversation.
        ///     This metadata will be propagated on MO callbacks associated with this conversation.
        ///     Up to 1024 characters long.
        ///     Note that the MO callback will always use the last metadata available in the conversation.
        ///     Important notes:   <br/><br/>
        ///     - If you send a message with the &#x60;conversation_metadata&#x60; field populated,
        ///     and then send another message without populating the &#x60;conversation_metadata&#x60; field,
        ///     the original metadata will continue be propagated on the related MO callbacks.  <br/><br/>
        ///     - If you send a message with the &#x60;conversation_metadata&#x60; field populated, and then
        ///     send another message with a different value for &#x60;conversation_metadata&#x60;
        ///     in the same conversation, the latest metadata value overwrites the existing one.
        ///     So, future MO callbacks will include the new metadata.  <br/><br/>
        ///     - The &#x60;conversation_metadata&#x60; only accepts json objects.
        ///     Currently only returned in the &#x60;message_metadata&#x60;
        ///     field of an [Inbound Message](/docs/conversation/callbacks/#inbound-message) callback.
        /// </summary>
        public JsonObject? ConversationMetadata { get; set; }


        /// <summary>
        ///     Identifies the recipient of the message. Requires either `contact_id` or `identified_by`.
        ///     If Dispatch Mode is used, only `identified_by` is allowed.
        /// </summary>
#if NET7_0_OR_GREATER
        public required IRecipient Recipient { get; set; }
#else
        public IRecipient Recipient { get; set; } = null!;
#endif


        /// <summary>
        ///     The timeout allotted for sending the message, expressed in seconds.
        ///     Passed to channels which support it and emulated by the Conversation API for channels without ttl
        ///     support but with message retract/unsend functionality. Channel failover will not be performed for
        ///     messages with an expired TTL. 
        /// </summary>
        [JsonPropertyName("ttl")]
        [JsonConverter(typeof(TimeToLiveConverter))]
        public int? TtlSeconds { get; set; }


        /// <summary>
        ///     Overrides the app&#39;s [Processing Mode](../../../../../conversation/processing-modes/). Default value is &#x60;DEFAULT&#x60;.
        /// </summary>
        public ProcessingStrategy? ProcessingStrategy { get; set; }


        /// <summary>
        ///     An arbitrary identifier that will be propagated to callbacks related to this message, including MO replies. Only applicable to messages sent with the &#x60;CONVERSATION&#x60; processing mode. Up to 128 characters long.
        /// </summary>
        public string? CorrelationId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SendMessageRequest {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  CallbackUrl: ").Append(CallbackUrl).Append("\n");
            sb.Append("  ChannelPriorityOrder: ").Append(ChannelPriorityOrder).Append("\n");
            sb.Append("  ChannelProperties: ").Append(ChannelProperties).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  MessageMetadata: ").Append(MessageMetadata).Append("\n");
            sb.Append("  ConversationMetadata: ").Append(ConversationMetadata).Append("\n");
            sb.Append("  Queue: ").Append(Queue).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("  Ttl: ").Append(TtlSeconds).Append("\n");
            sb.Append("  ProcessingStrategy: ").Append(ProcessingStrategy).Append("\n");
            sb.Append("  CorrelationId: ").Append(CorrelationId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class TimeToLiveConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str is null) return null;
            str = str.Trim();
            var val = str.TakeWhile(x => x == 's').ToString();
            if (int.TryParse(val, out int result))
            {
                return result;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            string? result = value.HasValue ? value.Value + "s" : null;
            JsonSerializer.Serialize(writer, result, options);
        }
    }
}
