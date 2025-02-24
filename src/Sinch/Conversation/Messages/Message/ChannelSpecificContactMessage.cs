using System;
using System.Text;
using System.Text.Json.Nodes;
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
        public required ChannelSpecificMessageType MessageType { get; set; }


        /// <summary>
        ///     Gets or Sets Message
        /// </summary>
        [JsonPropertyName("message")]
        public required ChannelSpecificMessageContent Message { get; set; }

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
        public required ChannelSpecificMessageType Type { get; set; }

        [JsonPropertyName("nfm_reply")]
        public required WhatsAppInteractiveNfmReply NfmReply { get; set; }
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
        public required NameEnum Name { get; set; }

        /// <summary>
        ///     The JSON specific data.
        /// </summary>
        [JsonPropertyName("response_json")]
        public required string ResponseJson { get; set; }


        /// <summary>
        ///     The message body.
        /// </summary>
        [JsonPropertyName("body")]
        public required string Body { get; set; }


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
