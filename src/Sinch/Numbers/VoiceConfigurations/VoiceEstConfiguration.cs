using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class VoiceEstConfiguration : VoiceConfiguration
    {
        /// <summary>
        ///     The trunk ID if the type is EST. The &#x60;trunkId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/trunks/your-trunks).
        /// </summary>
        [JsonPropertyName("trunkId")]
        public string? TrunkId { get; set; }

        /// <summary>
        ///     This object is temporary and will appear while the scheduled voice provisioning is processing.
        ///     Once it has successfully processed, only the ID of the Voice configuration will display.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("scheduledVoiceProvisioning")]
        public ScheduledVoiceEstProvisioning? ScheduledVoiceProvisioning { get; internal set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType Type { get; protected set; } = VoiceApplicationType.Est;
    }
}
