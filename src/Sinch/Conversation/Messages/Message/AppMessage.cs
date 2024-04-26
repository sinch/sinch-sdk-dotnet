using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    // ! IMPORTANT SERIALIZATION: maps every type of messages properties to Message 
    [JsonConverter(typeof(AppMessageConverter))]
    public class AppMessage
    {
        public IMessage? Message { get; set; }

        /// <summary>
        ///     Optional. Channel specific messages, overriding any transcoding.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, JsonValue>? ExplicitChannelMessage { get; set; }

        /// <summary>
        ///     Channel specific messages, overriding any transcoding.
        ///     The structure of this property is more well-defined than the open structure of
        ///     the explicit_channel_message property, and may be easier to use.
        ///     The key in the map must point to a valid conversation channel as defined in the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, IChannelSpecificMessage>? ChannelSpecificMessage { get; set; }


        public Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>? ExplicitChannelOmniMessage { get; set; }

        /// <inheritdoc cref="Agent" />        
        public Agent? Agent { get; set; }
    }

    public class AppMessageConverter : JsonConverter<AppMessage>
    {
        public override AppMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);

            foreach (var entry in MessagePropNameToTypeMap.Map)
            {
                if (elem.TryGetProperty(entry.Key, out var value))
                {
                    return new AppMessage()
                    {
                        Message = value.Deserialize<IMessage>(options),
                        Agent = elem.TryGetProperty("agent", out var agent) ? agent.Deserialize<Agent>(options) : null,
                        ChannelSpecificMessage =
                            elem.TryGetProperty("channel_specific_message", out var channelSpecificMessage)
                                ? channelSpecificMessage
                                    .Deserialize<Dictionary<ConversationChannel, IChannelSpecificMessage>?>(options)
                                : null,
                        ExplicitChannelMessage =
                            elem.TryGetProperty("explicit_channel_message", out var explicitChannelMessage)
                                ? explicitChannelMessage
                                    .Deserialize<Dictionary<ConversationChannel, JsonValue>?>(options)
                                : null,
                        ExplicitChannelOmniMessage = elem.TryGetProperty("explicit_channel_omni_message",
                            out var explicitChannelOmniMessage)
                            ? explicitChannelOmniMessage
                                .Deserialize<Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>?>(options)
                            : null
                    };
                }
            }

            throw new JsonException(
                $"Failed to match {nameof(IMessage)}, got json element: {elem.ToString()}");
        }

        public override void Write(Utf8JsonWriter writer, AppMessage value, JsonSerializerOptions options)
        {
            var messageType = value.Message?.GetType();
            var matchingType = MessagePropNameToTypeMap.Map.FirstOrDefault(x => x.Value == messageType);

            var obj = new Dictionary<string, object?>();
            if (matchingType.Key is not null)
            {
                obj.Add(matchingType.Key, value.Message);
            }

            if (options.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull
                or JsonIgnoreCondition.WhenWritingDefault or JsonIgnoreCondition.Always)
            {
                if (value.Agent is not null)
                {
                    obj.Add("agent", value.Agent);
                }

                if (value.ChannelSpecificMessage is not null)
                {
                    obj.Add("channel_specific_message", value.ChannelSpecificMessage);
                }


                if (value.ExplicitChannelMessage is not null)
                {
                    obj.Add("explicit_channel_message", value.ExplicitChannelMessage);
                }

                if (value.ExplicitChannelOmniMessage is not null)
                {
                    obj.Add("explicit_channel_omni_message", value.ExplicitChannelOmniMessage);
                }
            }
            else
            {
                obj.Add("agent", value.Agent);
                obj.Add("channel_specific_message", value.ChannelSpecificMessage);
                obj.Add("explicit_channel_message", value.ExplicitChannelMessage);
                obj.Add("explicit_channel_omni_message", value.ExplicitChannelOmniMessage);
            }


            JsonSerializer.Serialize(writer, obj, options);
        }
    }


    /// <summary>
    ///     A message containing a channel specific message (not supported by OMNI types).
    /// </summary>
    [JsonDerivedType(typeof(FlowMessage))]
    [JsonConverter(typeof(ChannelSpecificMessageJsonInterfaceConverter))]
    public interface IChannelSpecificMessage
    {
        /// <summary>
        ///     Gets or Sets MessageType
        /// </summary>
        public MessageType MessageType { get; }
    }


    [JsonConverter(typeof(EnumRecordJsonConverter<ChannelSpecificTemplate>))]
    public record ChannelSpecificTemplate(string Value) : EnumRecord(Value)
    {
        public static readonly ChannelSpecificTemplate WhatsApp = new ChannelSpecificTemplate("WHATSAPP");
        public static readonly ChannelSpecificTemplate KakaoTalk = new ChannelSpecificTemplate("KAKAOTALK");
        public static readonly ChannelSpecificTemplate WeChat = new ChannelSpecificTemplate("WECHAT");
    }

    public class ChannelSpecificMessageJsonInterfaceConverter : JsonConverter<IChannelSpecificMessage>
    {
        public override IChannelSpecificMessage Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            // not optimal but straightforward
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "message_type");
            var method = descriptor.Value.GetString();

            if (MessageType.Flows.Value == method)
                return elem.Deserialize<FlowMessage>(options) ??
                       throw new InvalidOperationException($"{nameof(FlowMessage)} deserialization result is null.");

            throw new JsonException(
                $"Failed to match {nameof(IChannelSpecificMessage)}, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IChannelSpecificMessage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

    public class FlowMessage : IChannelSpecificMessage
    {
        [JsonPropertyName("message_type")]
        [JsonInclude]
        public MessageType MessageType { get; private set; } = MessageType.Flows;

        [JsonPropertyName("message")]
        public FlowChannelSpecificMessage? Message { get; set; }
    }

    /// <summary>
    ///     Defines MessageType
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageType>))]
    public record MessageType(string Value) : EnumRecord(Value)
    {
        public static readonly MessageType Flows = new("FLOWS");
    }
}
