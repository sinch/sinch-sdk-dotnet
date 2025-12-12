using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Numbers.VoiceConfigurations;

namespace Sinch.Numbers
{
    public class ScheduledVoiceProvisioning
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
        [JsonInclude]
        // Expected to be set by the API response, or by Subclasses
        public virtual VoiceApplicationType? Type { get; internal set; }

        /// <summary>
        ///     The provisioning status. It will be either WAITING, IN_PROGRESS or FAILED.
        ///     If the provisioning fails, a reason for the failure will be provided.
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/error-codes/provisioning-errors/">
        ///         See a full list of provisioning error descriptions
        ///     </see>
        ///     and troubleshooting recommendations.
        /// </summary>
        public ProvisioningStatus? Status { get; set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        ///     Your app ID for the Voice API. The &#x60;appId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("appId")]
        [Obsolete($"Plain {nameof(ScheduledVoiceProvisioning)} will become abstract in future versions. Use concrete type of {nameof(ScheduledVoiceRtcProvisioning)}.")]
        public string? AppId { get; set; }
    }

    public sealed class ScheduledVoiceProvisioningConverter : JsonConverter<ScheduledVoiceProvisioning>
    {
        public override ScheduledVoiceProvisioning? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            if (elem.TryGetProperty("type", out var type))
            {
                var typeStr = type.GetString();
                if (typeStr == VoiceApplicationType.Fax.Value)
                {
                    return elem.Deserialize<ScheduledVoiceFaxProvisioning>(options);
                }

                if (typeStr == VoiceApplicationType.Rtc.Value)
                {
                    return elem.Deserialize<ScheduledVoiceRtcProvisioning>(options);
                }

                if (typeStr == VoiceApplicationType.Est.Value)
                {
                    return elem.Deserialize<ScheduledVoiceEstProvisioning>(options);
                }

                throw new JsonException(
                    $"Failed to match {nameof(ScheduledVoiceProvisioning)}. Type tag is invalid with value ${type}.");
            }

            throw new JsonException($"Failed to deserialize {nameof(ScheduledVoiceProvisioning)}");
        }

        public override void Write(Utf8JsonWriter writer, ScheduledVoiceProvisioning value,
            JsonSerializerOptions options)
        {
            if (value.Type == VoiceApplicationType.Fax)
            {
                JsonSerializer.Serialize(writer, value, typeof(ScheduledVoiceFaxProvisioning), options);
            }
            else if (value.Type == VoiceApplicationType.Est)
            {
                JsonSerializer.Serialize(writer, value, typeof(ScheduledVoiceEstProvisioning), options);
            }
            else if (value.Type == VoiceApplicationType.Rtc)
            {
                JsonSerializer.Serialize(writer, value, typeof(ScheduledVoiceRtcProvisioning), options);
            }
            else
            {
                throw new JsonException(
                    $"Failed to serialize {nameof(ScheduledVoiceProvisioning)}, unhandled type: {value.Type}");
            }
        }
    }
}
