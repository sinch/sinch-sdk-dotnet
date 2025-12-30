using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class VoiceRtcConfiguration : VoiceConfiguration
    {
        /// <summary>
        ///     Your app ID for the Voice API. The &#x60;appId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("appId")]
        public string? AppId { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType? Type { get; protected set; } = VoiceApplicationType.Rtc;
    }
}
