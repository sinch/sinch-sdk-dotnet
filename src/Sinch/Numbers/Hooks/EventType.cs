using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(SinchEnumConverter<EventType>))]
    public enum EventType
    {
        [EnumMember(Value = "PROVISIONING_TO_SMS_PLATFORM")]
        ProvisioningToSmsPlatform,
        [EnumMember(Value = "LINK_TO_10DLC_CAMPAIGN")]
        LinkTo10DlcCampaign
    }
}
