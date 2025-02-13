using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     A message type for sending WhatsApp Flows.
    /// </summary>
    public sealed class FlowChannelSpecificMessage
    {
        /// <summary>
        /// The mode in which the flow is.
        /// </summary>
        /// <value>The mode in which the flow is.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<FlowModeType>))]
        public record FlowModeType(string Value) : EnumRecord(Value)
        {
            public static readonly FlowModeType Draft = new("draft");
            public static readonly FlowModeType Published = new("published");
        }


        /// <summary>
        /// The mode in which the flow is.
        /// </summary>
        [JsonPropertyName("flow_mode")]
        public FlowModeType? FlowMode { get; set; }

        /// <summary>
        /// Defines FlowAction
        /// </summary>
        [JsonConverter(typeof(EnumRecordJsonConverter<FlowActionType>))]
        public record FlowActionType(string Value) : EnumRecord(Value)
        {
            public static readonly FlowActionType Navigate = new("navigate");
            public static readonly FlowActionType DataExchange = new("data_exchange");
        }


        /// <summary>
        /// Gets or Sets FlowAction
        /// </summary>
        [JsonPropertyName("flow_action")]
        public FlowActionType? FlowAction { get; set; }

        /// <summary>
        ///     Gets or Sets Header
        /// </summary>
        [JsonPropertyName("header")]
        public IFlowChannelSpecificMessageHeader? Header { get; set; }


        /// <summary>
        ///     Gets or Sets Body
        /// </summary>
        [JsonPropertyName("body")]
        public WhatsAppInteractiveBody? Body { get; set; }


        /// <summary>
        ///     Gets or Sets Footer
        /// </summary>
        [JsonPropertyName("footer")]
        public WhatsAppInteractiveFooter? Footer { get; set; }


        /// <summary>
        ///     ID of the Flow.
        /// </summary>
        [JsonPropertyName("flow_id")]

        public required string FlowId { get; set; }



        /// <summary>
        ///     Generated token which is an identifier.
        /// </summary>
        [JsonPropertyName("flow_token")]
        public string? FlowToken { get; set; }


        /// <summary>
        ///     Text which is displayed on the Call To Action button (20 characters maximum, emoji not supported).
        /// </summary>
        [JsonPropertyName("flow_cta")]

        public required string FlowCta { get; set; }



        /// <summary>
        ///     Gets or Sets FlowActionPayload
        /// </summary>
        [JsonPropertyName("flow_action_payload")]
        public FlowChannelSpecificMessageFlowActionPayload? FlowActionPayload { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(FlowChannelSpecificMessage)} {{\n");
            sb.Append($"  {nameof(Header)}: ").Append(Header).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(Footer)}: ").Append(Footer).Append('\n');
            sb.Append($"  {nameof(FlowId)}: ").Append(FlowId).Append('\n');
            sb.Append($"  {nameof(FlowToken)}: ").Append(FlowToken).Append('\n');
            sb.Append($"  {nameof(FlowMode)}: ").Append(FlowMode).Append('\n');
            sb.Append($"  {nameof(FlowCta)}: ").Append(FlowCta).Append('\n');
            sb.Append($"  {nameof(FlowAction)}: ").Append(FlowAction).Append('\n');
            sb.Append($"  {nameof(FlowActionPayload)}: ").Append(FlowActionPayload).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Header of the interactive message with text.
    /// </summary>
    public sealed class WhatsAppInteractiveTextHeader : IFlowChannelSpecificMessageHeader
    {
        /// <summary>
        ///     Must be set to text.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        public WhatsAppHeaderType Type { get; private set; } = WhatsAppHeaderType.Text;


        /// <summary>
        ///     Text for the header. Formatting allows emojis, but not Markdown.
        /// </summary>
        [JsonPropertyName("text")]

        public required string Text { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveTextHeader)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Text)}: ").Append(Text).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Header of the interactive message with image.
    /// </summary>
    public sealed class WhatsAppInteractiveImageHeader : IFlowChannelSpecificMessageHeader
    {
        [JsonInclude]
        [JsonPropertyName("type")]
        public WhatsAppHeaderType Type { get; private set; } = WhatsAppHeaderType.Image;


        /// <summary>
        ///     Gets or Sets Image
        /// </summary>
        [JsonPropertyName("image")]

        public required WhatsAppInteractiveHeaderMedia Image { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveImageHeader)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Image)}: ").Append(Image).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Media object for the header.
    /// </summary>
    public sealed class WhatsAppInteractiveHeaderMedia
    {
        /// <summary>
        ///     URL for the media.
        /// </summary>
        [JsonPropertyName("link")]

        public required string Link { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveHeaderMedia)} {{\n");
            sb.Append($"  {nameof(Link)}: ").Append(Link).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Header of the interactive message with document.
    /// </summary>
    public sealed class WhatsAppInteractiveDocumentHeader : IFlowChannelSpecificMessageHeader
    {
        /// <summary>
        ///     Must be set to document.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        public WhatsAppHeaderType Type { get; private set; } = WhatsAppHeaderType.Document;


        /// <summary>
        ///     Gets or Sets Document
        /// </summary>
        [JsonPropertyName("document")]

        public required WhatsAppInteractiveHeaderMedia Document { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveDocumentHeader)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Document)}: ").Append(Document).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Header of the interactive message with video.
    /// </summary>
    public sealed class WhatsAppInteractiveVideoHeader : IFlowChannelSpecificMessageHeader
    {
        /// <summary>
        ///     Must be set to video.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        public WhatsAppHeaderType Type { get; private set; } = WhatsAppHeaderType.Video;


        /// <summary>
        ///     Gets or Sets Video
        /// </summary>
        [JsonPropertyName("video")]

        public required WhatsAppInteractiveHeaderMedia Video { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveVideoHeader)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Video)}: ").Append(Video).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
