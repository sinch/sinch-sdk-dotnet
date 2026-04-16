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
    ///             <item>Assign a value — sent with that value.</item>
    ///             <item>Assign <c>null</c> — field cleared on server.</item>
    ///             <item>Leave unassigned (default) — field omitted from the request entirely.</item>
    ///         </list>
    ///         Fields that support null-as-clear use the Stripe-style <see cref="SetTracker" /> pattern
    ///         internally; callers see only plain nullable types.
    ///     </para>
    /// </summary>
    public sealed class UpdateActiveNumberRequest : IHasSetTracker
    {
        private readonly SetTracker _setTracker = new();
        SetTracker IHasSetTracker.SetTracker => _setTracker;
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
        private SmsConfiguration? _smsConfiguration;

        public SmsConfiguration? SmsConfiguration
        {
            get => _smsConfiguration;
            set
            {
                _setTracker.Track();
                _smsConfiguration = value;
            }
        }

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
