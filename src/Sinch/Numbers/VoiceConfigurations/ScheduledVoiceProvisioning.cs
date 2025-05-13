using System;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.VoiceConfigurations
{
    public abstract class ScheduledVoiceProvisioning
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
        [JsonInclude]
        // Expected to be set by the API response, or by Subclasses
        public virtual VoiceApplicationType? Type { get; internal set; }

        /// <summary>
        ///     The provisioning status. It will be either WAITING, IN_PROGRESS or FAILED.
        ///     If the provisioning fails, a reason for the failure will be provided.
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/error-codes/provisioning-errors/">
        ///         See a full list of provisioning error descriptions
        ///     </see>
        ///     and troubleshooting recommendations.
        /// </summary>
        public ProvisioningStatus? Status { get; set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }
    }
}
