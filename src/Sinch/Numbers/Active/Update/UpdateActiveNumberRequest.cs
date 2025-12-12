using System.Text.Json.Serialization;

namespace Sinch.Numbers.Active.Update
{
    public sealed class UpdateActiveNumberRequest
    {
        /// <summary>
        ///     User supplied name for the phone number.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        ///     The current SMS configuration for this number.<br /><br />
        ///     Once the servicePlanId is sent, it enters scheduled provisioning. <br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully,
        ///     the servicePlanId sent will appear directly under the smsConfiguration object.
        /// </summary>
        public SmsConfiguration? SmsConfiguration { get; set; }

        /// <summary>
        ///     The current voice configuration for this number.
        ///     During scheduled provisioning, the app ID value may be empty in a response if it is still processing or if it has
        ///     failed.
        ///     The status of scheduled provisioning will show under a scheduledVoiceProvisioning object if it's still running.
        ///     Once processed successfully, the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        [JsonConverter(typeof(VoiceConfigurationConverter))]
        public VoiceConfiguration? VoiceConfiguration { get; set; }

        /// <summary>
        ///     The callback URL to be called for a rented number's provisioning / deprovisioning operations.
        /// </summary>
        public string? CallbackUrl { get; set; }
    }
}
