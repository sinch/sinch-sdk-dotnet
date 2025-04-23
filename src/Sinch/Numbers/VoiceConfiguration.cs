using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Numbers.VoiceConfigurations
{
    public class VoiceConfiguration
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
        [JsonInclude]
        // Expected to be set by the API response, or by Subclasses
        public virtual VoiceApplicationType? Type { get; protected set; } = VoiceApplicationType.Rtc;

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("lastUpdatedTime")]
        public DateTime? LastUpdatedTime { get; internal set; }

        [JsonPropertyName("appId")]
        [Obsolete(
            $"Plain {nameof(VoiceConfiguration)} will become abstract in future versions. Use concrete type of {nameof(VoiceRtcConfiguration)} instead.")]
        public string? AppId { get; set; }

        [JsonPropertyName("scheduledVoiceProvisioning")]
        [JsonInclude]
        [Obsolete($"Will be removed in future versions." +
                  $" See specific {nameof(ScheduledVoiceRtcProvisioning)}, {nameof(ScheduledVoiceEstProvisioning)}, or {nameof(ScheduledVoiceFaxProvisioning)}" +
                  $" in corresponding classes: {nameof(VoiceRtcConfiguration)}, {nameof(VoiceEstConfiguration)}, or {nameof(VoiceFaxConfiguration)}.")]
        public ScheduledVoiceProvisioning? ScheduledVoiceProvisioning { get; internal set; }
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
                    // TODO: remove it in 2.0
                    // keeping it for backward compatility of VoiceConfiguraiton
                    var result = elem.Deserialize<VoiceRtcConfiguration>(options);
#pragma warning disable CS0618 // Type or member is obsolete
                    var voiceConfiguration = (result as VoiceConfiguration)!;
                    voiceConfiguration!.AppId = result!.AppId;
                    voiceConfiguration.ScheduledVoiceProvisioning = result.ScheduledVoiceProvisioning;
                    voiceConfiguration.ScheduledVoiceProvisioning!.AppId = result.AppId;
#pragma warning restore CS0618 // Type or member is obsolete
                    return result;
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
                // TODO: remove in 2.0
                // if base VoiceConfiguration is passed, serialize it as RTC configuration for backward compatiblity
                if (value.GetType() == typeof(VoiceConfiguration))
                {
                    var voiceRtcConfiguration = new VoiceRtcConfiguration()
                    {
                        AppId = value.AppId
                    };
                    JsonSerializer.Serialize(writer, voiceRtcConfiguration, typeof(VoiceRtcConfiguration), options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, value, typeof(VoiceRtcConfiguration), options);
                }
            }
            else
            {
                throw new JsonException(
                    $"Failed to serialize {nameof(VoiceConfiguration)}, unhandled type: {value.Type}");
            }
        }
    }
}
