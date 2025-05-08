using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class ScheduledVoiceRtcProvisioning : ScheduledVoiceProvisioning
    {
        [JsonConstructor]
        public ScheduledVoiceRtcProvisioning()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            base.AppId = AppId;
#pragma warning restore CS0618 // Type or member is obsolete
        }
        /// <summary>
        ///     Your app ID for the Voice API. The &#x60;appId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("appId")]
        public new string? AppId { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType? Type { get; internal set; } = VoiceApplicationType.Rtc;
    }
}
