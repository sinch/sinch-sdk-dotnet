using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Numbers.VoiceConfigurations
{
    public abstract class VoiceConfiguration
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
        [JsonInclude]
        // Expected to be set by the API response, or by Subclasses
        public abstract VoiceApplicationType Type { get; protected set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("lastUpdatedTime")]
        public DateTime? LastUpdatedTime { get; internal set; }
    }


    public sealed class VoiceConfigurationConverter : JsonConverter<VoiceConfiguration>
    {
        public override VoiceConfiguration? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            if (elem.TryGetProperty("type", out var type))
            {
                var typeStr = type.GetString();
                if (typeStr == VoiceApplicationType.Fax.Value)
                {
                    return elem.Deserialize<VoiceFaxConfiguration>(options);
                }

                if (typeStr == VoiceApplicationType.Rtc.Value)
                {
                    return elem.Deserialize<VoiceRtcConfiguration>(options);
                }

                if (typeStr == VoiceApplicationType.Est.Value)
                {
                    return elem.Deserialize<VoiceEstConfiguration>(options);
                }

                throw new JsonException(
                    $"Failed to match {nameof(VoiceConfiguration)}. Type tag is invalid with value ${type}.");
            }

            throw new JsonException($"Failed to deserialize {nameof(VoiceConfiguration)}");
        }

        public override void Write(Utf8JsonWriter writer, VoiceConfiguration value, JsonSerializerOptions options)
        {
            if (value.Type == VoiceApplicationType.Fax)
            {
                JsonSerializer.Serialize(writer, value, typeof(VoiceFaxConfiguration), options);
            }
            else if (value.Type == VoiceApplicationType.Est)
            {
                JsonSerializer.Serialize(writer, value, typeof(VoiceEstConfiguration), options);
            }
            else if (value.Type == VoiceApplicationType.Rtc)
            {
                JsonSerializer.Serialize(writer, value, typeof(VoiceRtcConfiguration), options);
            }
            else
            {
                throw new JsonException(
                    $"Failed to serialize {nameof(VoiceConfiguration)}, unhandled type: {value.Type}");
            }
        }
    }
}
