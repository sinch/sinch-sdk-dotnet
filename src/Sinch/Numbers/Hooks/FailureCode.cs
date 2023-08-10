using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(SinchEnumConverter<FailureCode>))]
    public enum FailureCode
    {
        [EnumMember(Value = "CAMPAIGN_NOT_AVAILABLE")]
        CampaignNotAvailable,
    
        [EnumMember(Value = "EXCEEDED_10DLC_LIMIT")]
        Exceeded10DlcLimit,
    
        [EnumMember(Value = "NUMBER_PROVISIONING_FAILED")]
        NumberProvisioningFailed,
    
        [EnumMember(Value = "PARTNER_SERVICE_UNAVAILABLE")]
        PartnerServiceUnavailable,
    
        [EnumMember(Value = "CAMPAIGN_PENDING_ACCEPTANCE")]
        CampaignPendingAcceptance,
    
        [EnumMember(Value = "MNO_SHARING_ERROR")]
        MnoSharingError,
    
        [EnumMember(Value = "CAMPAIGN_PROVISIONING_FAILED")]
        CampaignProvisioningFailed,
    
        [EnumMember(Value = "CAMPAIGN_EXPIRED")]
        CampaignExpired,
    
        [EnumMember(Value = "CAMPAIGN_MNO_REJECTED")]
        CampaignMnoRejected,
    
        [EnumMember(Value = "CAMPAIGN_MNO_SUSPENDED")]
        CampaignMnoSuspended,
    
        [EnumMember(Value = "CAMPAIGN_MNO_REVIEW")]
        CampaignMnoReview,
    
        [EnumMember(Value = "INSUFFICIENT_BALANCE")]
        InsufficientBalance,
    
        [EnumMember(Value = "MOCK_CAMPAIGN_NOT_ALLOWED")]
        MockCampaignNotAllowed,
    
        [EnumMember(Value = "TFN_NOT_ALLOWED")]
        TfnNotAllowed,
    
        [EnumMember(Value = "INVALID_NNID")]
        InvalidNnid
    }
}
