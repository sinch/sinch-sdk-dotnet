using System;
using System.Collections.Generic;

namespace Sinch.Numbers
{
    public sealed class ScheduledProvisioning
    {
        /// <summary>
        ///     Service plan of the scheduled provisioning task.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ServicePlanId { get; set; }
#else
        public string ServicePlanId { get; set; }
#endif

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
        ///     The provisioning status codes are found
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/error-codes/provisioning-errors/">here</see>.
        /// </summary>
        // TODO?: provide error codes in enum
        public List<string> ErrorCodes { get; set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        ///     Campaign ID of the scheduled provisioning task.
        ///     Note that the campaign ID is only for US numbers as it relates to
        ///     <see href="https://community.sinch.com/t5/10DLC/What-is-The-Campaign-Registry-TCR/ta-p/7024">10DLC</see>.
        /// </summary>
        public string CampaignId { get; set; }
    }
}
