using System;

namespace Sinch.Numbers
{
    public sealed class ScheduledVoiceProvisioning
    {
        /// <summary>
        ///     <see href="https://community.sinch.com/t5/Glossary/RTC/ta-p/7699">RTC</see>
        ///     application ID of the scheduled provisioning task.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        ///     The provisioning status. It will be either WAITING, IN_PROGRESS or FAILED.
        ///     If the provisioning fails, a reason for the failure will be provided.
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/error-codes/provisioning-errors/">
        ///         See a full list of provisioning error descriptions
        ///     </see>
        ///     and troubleshooting recommendations.
        /// </summary>
        public ProvisioningStatus Status { get; set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }
    }
}
