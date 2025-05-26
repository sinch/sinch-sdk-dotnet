using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    public sealed class ChannelSpecificContactMessage
    {
        /// <summary>
        /// The message type.
        /// </summary>
        [JsonPropertyName("message_type")]
#if NET7_0_OR_GREATER
        public required ChannelSpecificMessageType MessageType { get; set; }
#else
        public ChannelSpecificMessageType MessageType { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets Message
        /// </summary>
        [JsonPropertyName("message")]
#if NET7_0_OR_GREATER
        public required ChannelSpecificMessageContent Message { get; set; }
#else
        public ChannelSpecificMessageContent Message { get; set; } = null!;
#endif

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ChannelSpecificContactMessage)} {{\n");
            sb.Append($"  {nameof(MessageType)}: ").Append(MessageType).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public sealed class ChannelSpecificMessageContent
    {
        [JsonPropertyName("type")]
#if NET7_0_OR_GREATER
        public required ChannelSpecificMessageType Type { get; set; }
#else
        public ChannelSpecificMessageType Type { get; set; } = null!;
#endif

        [JsonPropertyName("nfm_reply")]
#if NET7_0_OR_GREATER
        public required WhatsAppInteractiveNfmReply NfmReply { get; set; }
#else
        public WhatsAppInteractiveNfmReply NfmReply { get; set; } = null!;
#endif
    }

    /// <summary>
    ///     The message type.
    /// </summary>
    /// <value>The message type.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<ChannelSpecificMessageType>))]
    public record ChannelSpecificMessageType(string Value) : EnumRecord(Value)
    {
        public static readonly ChannelSpecificMessageType NfmReply = new("nfm_reply");
    }

    /// <summary>
    ///     The interactive nfm reply message.
    /// </summary>
    public sealed class WhatsAppInteractiveNfmReply
    {
        /// <summary>
        /// The nfm reply message type.
        /// </summary>
        /// <value>The nfm reply message type.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<NameEnum>))]
        public record NameEnum(string Value) : EnumRecord(Value)
        {
            public static readonly NameEnum Flow = new("flow");
            public static readonly NameEnum AddressMessage = new("address_message");
        }


        /// <summary>
        /// The nfm reply message type.
        /// </summary>
        [JsonPropertyName("name")]
#if NET7_0_OR_GREATER
        public required NameEnum Name { get; set; }
#else
        public NameEnum Name { get; set; } = null!;
#endif

        /// <summary>
        ///     The JSON specific data.
        /// </summary>
        [JsonPropertyName("response_json")]
#if NET7_0_OR_GREATER
        public required string ResponseJson { get; set; }
#else
        public string ResponseJson { get; set; } = null!;
#endif


        /// <summary>
        ///     The message body.
        /// </summary>
        [JsonPropertyName("body")]
#if NET7_0_OR_GREATER
        public required string Body { get; set; }
#else
        public string Body { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveNfmReply)} {{\n");
            sb.Append($"  {nameof(Name)}: ").Append(Name).Append('\n');
            sb.Append($"  {nameof(ResponseJson)}: ").Append(ResponseJson).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
