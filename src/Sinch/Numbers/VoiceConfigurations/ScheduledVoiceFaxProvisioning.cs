using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public sealed class ScheduledVoiceFaxProvisioning : ScheduledVoiceProvisioning
    {
        /// <summary>
        ///     The service ID if the type is FAX.
        ///     The &#x60;serviceId&#x60; can be found in your [Sinch Customer Dashboard](https://dashboard.sinch.com/fax/services).
        /// </summary>
        [JsonPropertyName("serviceId")]
        public string? ServiceId { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        internal override VoiceApplicationType? Type { get; set; } = VoiceApplicationType.Fax;
    }
}
