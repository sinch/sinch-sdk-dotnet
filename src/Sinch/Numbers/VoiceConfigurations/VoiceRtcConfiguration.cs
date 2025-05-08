using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class VoiceRtcConfiguration : VoiceConfiguration
    {
        /// <summary>
        ///     Your app ID for the Voice API. The &#x60;appId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("appId")]
        public new string? AppId { get; set; }

        /// <summary>
        ///     This object is temporary and will appear while the scheduled voice provisioning is processing.
        ///     Once it has successfully processed, only the ID of the Voice configuration will display.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("scheduledVoiceProvisioning")]
        public new ScheduledVoiceRtcProvisioning? ScheduledVoiceProvisioning { get; internal set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType? Type { get; protected set; } = VoiceApplicationType.Rtc;
    }
}
