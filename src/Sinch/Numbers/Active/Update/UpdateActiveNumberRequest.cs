using Sinch.Core;
using Sinch.Numbers.VoiceConfigurations;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.Active.Update
{
    /// <summary>
    ///     Request to update an active number (partial update — RFC 7396 JSON Merge Patch).
    ///     <para>
    ///         Three states per field:
    ///         <list type="bullet">
    ///             <item><c>DisplayName = "name"</c> — sent with that value; <c>DisplayName = null</c> — cleared on server.</item>
    ///             <item>Assign an <see cref="Optional{T}.Value" /> to other fields to send a value,
    ///                 <see cref="Optional{T}.CreateNull" /> to clear on server.</item>
    ///             <item>Leave unassigned (default <c>null</c>) — field omitted from the request entirely.</item>
    ///         </list>
    ///     </para>
    /// </summary>
    public sealed class UpdateActiveNumberRequest
    {
        [JsonInclude]
        [JsonPropertyName("displayName")]
        private Optional<string> _displayName = Optional<string>.CreateUnset();

        /// <summary>
        ///     User supplied name for the phone number.
        /// </summary>
        [JsonIgnore]
        public string? DisplayName
        {
            get => (_displayName as Optional<string>.Value)?.Data;
            set => _displayName = value == null ? Optional<string>.CreateNull() : Optional<string>.CreateValue(value);
        }

        /// <summary>
        ///     The current SMS configuration for this number.<br /><br />
        ///     Once the servicePlanId is sent, it enters scheduled provisioning. <br /><br />
        ///     The status of scheduled provisioning will show under a scheduledProvisioning object if it's still running. Once
        ///     processed successfully,
        ///     the servicePlanId sent will appear directly under the smsConfiguration object.
        /// </summary>
        public Optional<SmsConfiguration> SmsConfiguration { get; set; } = default!;

        /// <summary>
        ///     The current voice configuration for this number.
        ///     During scheduled provisioning, the app ID value may be empty in a response if it is still processing or if it has
        ///     failed.
        ///     The status of scheduled provisioning will show under a scheduledVoiceProvisioning object if it's still running.
        ///     Once processed successfully, the appId sent will appear directly under the voiceConfiguration object.
        /// </summary>
        [JsonConverter(typeof(OptionalVoiceConfigurationConverter))]
        public Optional<VoiceConfiguration> VoiceConfiguration { get; set; } = default!;

        /// <summary>
        ///     The callback URL to be called for a rented number's provisioning / deprovisioning operations.
        /// </summary>
        public Optional<string> CallbackUrl { get; set; } = default!;
    }
}
