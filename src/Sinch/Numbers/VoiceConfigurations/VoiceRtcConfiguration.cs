using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class VoiceRtcConfiguration : VoiceConfiguration
    {
        private string? _appId;

        /// <summary>
        ///     Your app ID for the Voice API. The &#x60;appId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("appId")]
        public new string? AppId
        {
            get => _appId;
            set
            {
                _appId = value;
                // TODO: remove in 2.0, keeping for backward compatiblity
#pragma warning disable CS0618 // Type or member is obsolete
                base.AppId = value;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        [JsonPropertyName("type")]
        [JsonInclude]
        public override VoiceApplicationType? Type { get; protected set; } = VoiceApplicationType.Rtc;
    }
}
