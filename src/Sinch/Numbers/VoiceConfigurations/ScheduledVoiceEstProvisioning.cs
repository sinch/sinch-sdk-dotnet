using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class ScheduledVoiceEstProvisioning : ScheduledVoiceProvisioning
    {
        /// <summary>
        ///     The trunk ID if the type is EST.
        ///     The &#x60;trunkId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/trunks/your-trunks).
        /// </summary>
        [JsonPropertyName("trunkId")]
        public string? TrunkId { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        internal override VoiceApplicationType? Type { get; set; } = VoiceApplicationType.Est;
    }
}
