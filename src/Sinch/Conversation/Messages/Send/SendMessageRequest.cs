using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;
using Sinch.Core;

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

        public required string AppId { get; set; }


        /// <summary>
        ///     Select the priority type for the message
        /// </summary>
        public MessageQueue? Queue { get; set; }


        /// <summary>
        ///     Overwrites the default callback url for delivery receipts for this message.
        ///     Note that you may
        ///     [define a `secret_for_overridden_callback_urls` at the app level](https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/App/operation/App_UpdateApp!path=callback_settings/secret_for_overridden_callback_urls&amp;t=request)
        ///     this secret will be used to sign the contents of delivery receipts when the default
        ///     callback URL is overridden by this property. The REST URL should be of the form: `http://host[:port]/path`
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



        public required AppMessage Message { get; set; }



        /// <summary>
        ///     Metadata that should be associated with the message. Returned in the `metadata` field of a
        ///     [Message Delivery Receipt](https://developers.sinch.com/docs/conversation/callbacks/#message-delivery-receipt).
        ///     Up to 1024 characters long.
        /// </summary>
        public string? MessageMetadata { get; set; }


        /// <summary>
        ///  Metadata that will be associated with the conversation in `CONVERSATION` mode and with the specified recipient identities in `DISPATCH` mode.
        ///  This metadata will be propagated on MO callbacks associated
        ///  with the respective conversation or user identity. Up to 2048 characters long.
        ///  Note that the MO callback will always use the last metadata available.<br /><br />
        ///  Important notes:<br />
        ///     - If you send a message with the `conversation_metadata` field populated, and then send another message without populating the `conversation_metadata` field, the original metadata will continue be propagated on the related MO callbacks.<br />
        ///     - If you send a message with the `conversation_metadata` field populated, and then send another message with a different value for `conversation_metadata` in the same conversation, the latest metadata value overwrites the existing one. So, future MO callbacks will include the new metadata.<br />
        ///     - The `conversation_metadata` only accepts json objects.<br />
        ///   Currently only returned in the `message_metadata` field of an [Inbound Message](https://developers.sinch.com/docs/conversation/callbacks/#inbound-message) callback.
        /// </summary>
        public JsonObject? ConversationMetadata { get; set; }


        /// <summary>
        ///     Identifies the recipient of the message. Requires either `contact_id` or `identified_by`.
        ///     If Dispatch Mode is used, only `identified_by` is allowed.
        /// </summary>

        public required IRecipient Recipient { get; set; }



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
        ///     An arbitrary identifier that will be propagated to callbacks related to this message, including MO messages from the recipient. The &#x60;correlation_id&#x60; is associated with the conversation in &#x60;CONVERSATION&#x60; mode and with the specified recipient identities in &#x60;DISPATCH&#x60; mode. The MO callbacks will always include the last &#x60;correlation_id&#x60; available, (which is similar to how the &#x60;conversation_metadata&#x60; property functions). Up to 128 characters long.
        /// </summary>
        public string? CorrelationId { get; set; }


        /// <summary>
        ///     Gets or Sets MessageContentType
        /// </summary>
        [JsonPropertyName("message_content_type")]
        public MessageContentType? MessageContentType { get; set; }

        /// <summary>
        ///     Gets or Sets ConversationMetadataUpdateStrategy
        /// </summary>
        [JsonPropertyName("conversation_metadata_update_strategy")]
        public MetadataUpdateStrategy? ConversationMetadataUpdateStrategy { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(SendMessageRequest)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(CallbackUrl)}: ").Append(CallbackUrl).Append('\n');
            sb.Append($"  {nameof(ChannelPriorityOrder)}: ").Append(ChannelPriorityOrder).Append('\n');
            sb.Append($"  {nameof(ChannelProperties)}: ").Append(ChannelProperties).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(ConversationMetadata)}: ").Append(ConversationMetadata).Append('\n');
            sb.Append($"  {nameof(Queue)}: ").Append(Queue).Append('\n');
            sb.Append($"  {nameof(Recipient)}: ").Append(Recipient).Append('\n');
            sb.Append($"  {nameof(TtlSeconds)}: ").Append(TtlSeconds).Append('\n');
            sb.Append($"  {nameof(ProcessingStrategy)}: ").Append(ProcessingStrategy).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(ConversationMetadataUpdateStrategy)}: ").Append(ConversationMetadataUpdateStrategy)
                .Append('\n');
            sb.Append($"  {nameof(MessageContentType)}: ").Append(MessageContentType).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     This field classifies the message content for use with Sinch&#39;s [consent management functionality](https://developers.sinch.com/docs/conversation/consent-management/). Note that this field is currently only used with Sinch&#39;s consent management functionality, and is not referenced elsewhere by the Conversation API.
    /// </summary>
    /// <value>This field classifies the message content for use with Sinch&#39;s [consent management functionality](https://developers.sinch.com/docs/conversation/consent-management/). Note that this field is currently only used with Sinch&#39;s consent management functionality, and is not referenced elsewhere by the Conversation API.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageContentType>))]
    public sealed record MessageContentType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default content type, when the content is not clearly defined, can be any type of content.
        /// </summary>
        public static readonly MessageContentType ContentUnknown = new("CONTENT_UNKNOWN");

        /// <summary>
        ///     Type that indicates that the content is related to Marketing, like marketing campaign messages.
        /// </summary>
        public static readonly MessageContentType ContentMarketing = new("CONTENT_MARKETING");

        /// <summary>
        ///      Type that indicates that the content is related to Notifications, like charges and alerts.
        /// </summary>
        public static readonly MessageContentType ContentNotification = new("CONTENT_NOTIFICATION");
    }

    /// <summary>
    ///     Update strategy for the &#x60;conversation_metadata&#x60; field. Only supported in &#x60;CONVERSATION&#x60; processing mode.
    /// </summary>
    /// <value>Update strategy for the &#x60;conversation_metadata&#x60; field. Only supported in &#x60;CONVERSATION&#x60; processing mode.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<MetadataUpdateStrategy>))]
    public sealed record MetadataUpdateStrategy(string Value) : EnumRecord(Value)
    {
        public static readonly MetadataUpdateStrategy Replace = new("REPLACE");
        public static readonly MetadataUpdateStrategy MergePatch = new("MERGE_PATCH");
    }

    public sealed class TimeToLiveConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str is null) return null;
            str = str.Trim();
            var val = new string(str.TakeWhile(char.IsNumber).ToArray());
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
