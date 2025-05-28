using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    /// <summary>
    ///     Represents the failure code options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<FailureCode>))]
    public record FailureCode(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Campaign is not available.
        /// </summary>
        public static readonly FailureCode CampaignNotAvailable = new("CAMPAIGN_NOT_AVAILABLE");

        /// <summary>
        ///     Exceeded 10 DLC limit.
        /// </summary>
        public static readonly FailureCode Exceeded10DlcLimit = new("EXCEEDED_10DLC_LIMIT");

        /// <summary>
        ///     Number provisioning failed.
        /// </summary>
        public static readonly FailureCode NumberProvisioningFailed = new("NUMBER_PROVISIONING_FAILED");

        /// <summary>
        ///     Partner service is unavailable.
        /// </summary>
        public static readonly FailureCode PartnerServiceUnavailable = new("PARTNER_SERVICE_UNAVAILABLE");

        /// <summary>
        ///     Campaign is pending acceptance.
        /// </summary>
        public static readonly FailureCode CampaignPendingAcceptance = new("CAMPAIGN_PENDING_ACCEPTANCE");

        /// <summary>
        ///     MNO sharing error.
        /// </summary>
        public static readonly FailureCode MnoSharingError = new("MNO_SHARING_ERROR");

        /// <summary>
        ///     Campaign provisioning failed.
        /// </summary>
        public static readonly FailureCode CampaignProvisioningFailed = new("CAMPAIGN_PROVISIONING_FAILED");

        /// <summary>
        ///     Campaign expired.
        /// </summary>
        public static readonly FailureCode CampaignExpired = new("CAMPAIGN_EXPIRED");

        /// <summary>
        ///     Campaign MNO rejected.
        /// </summary>
        public static readonly FailureCode CampaignMnoRejected = new("CAMPAIGN_MNO_REJECTED");

        /// <summary>
        ///     Campaign MNO suspended.
        /// </summary>
        public static readonly FailureCode CampaignMnoSuspended = new("CAMPAIGN_MNO_SUSPENDED");

        /// <summary>
        ///     Campaign MNO review.
        /// </summary>
        public static readonly FailureCode CampaignMnoReview = new("CAMPAIGN_MNO_REVIEW");

        /// <summary>
        ///     Insufficient balance.
        /// </summary>
        public static readonly FailureCode InsufficientBalance = new("INSUFFICIENT_BALANCE");

        /// <summary>
        ///     Mock campaign is not allowed.
        /// </summary>
        public static readonly FailureCode MockCampaignNotAllowed = new("MOCK_CAMPAIGN_NOT_ALLOWED");

        /// <summary>
        ///     TFN (Toll-Free Number) is not allowed.
        /// </summary>
        public static readonly FailureCode TfnNotAllowed = new("TFN_NOT_ALLOWED");

        /// <summary>
        ///     Invalid NNID (Numbering Plan ID).
        /// </summary>
        public static readonly FailureCode InvalidNnid = new("INVALID_NNID");

        public static readonly FailureCode ProvisioningToVoicePlatformFailed = new("PROVISIONING_TO_VOICE_PLATFORM_FAILED");
    }
}
