using System;
using System.Text.Json.Serialization;

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
        internal virtual VoiceApplicationType Type { get; set; } = VoiceApplicationType.Rtc;

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("lastUpdatedTime")]
        public DateTime? LastUpdatedTime { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("scheduledVoiceProvisioning")]
        [JsonConverter(typeof(ScheduledVoiceProvisioningConverter))]
        public ScheduledVoiceProvisioning? ScheduledVoiceProvisioning { get; internal set; }
    }
}
