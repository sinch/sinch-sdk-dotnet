using Sinch.Numbers.VoiceConfigurations;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.Available.Rent
{
    public sealed class RentActiveNumberRequest
    {
        /// <summary>
        ///     The current SMS configuration for this number. <br /><br />
        ///     Once the servicePlanId is sent, it enters scheduled provisioning.<br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully, the servicePlanId sent will appear directly under the smsConfiguration object.
        /// </summary>
        [JsonPropertyName("smsConfiguration")]
        public SmsConfiguration? SmsConfiguration { get; set; }

        /// <summary>
        ///     The current voice configuration for this number. During scheduled provisioning,
        ///     the app ID value may be empty in a response if it is still processing or if it has failed.
        ///     The status of scheduled provisioning will show under a scheduledVoiceProvisioning object
        ///     if it's still running. Once processed successfully,
        ///     the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        [JsonConverter(typeof(VoiceConfigurationConverter))]
        [JsonPropertyName("voiceConfiguration")]
        public VoiceConfiguration? VoiceConfiguration { get; set; }
    }
}
