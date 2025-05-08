using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class VoiceFaxConfiguration : VoiceConfiguration
    {
        /// <summary>
        ///     This object is temporary and will appear while the scheduled voice provisioning is processing.
        ///     Once it has successfully processed, only the ID of the Voice configuration will display.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("scheduledVoiceProvisioning")]
        public ScheduledVoiceFaxProvisioning? ScheduledVoiceProvisioning { get; internal set; }

        /// <summary>
        ///     The service ID if the type is FAX. The &#x60;serviceId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/fax/services).
        /// </summary>
        [JsonPropertyName("serviceId")]
        public string? ServiceId { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType? Type { get; protected set; } = VoiceApplicationType.Fax;
    }
}
