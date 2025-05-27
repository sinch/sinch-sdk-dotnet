using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(EnumRecordJsonConverter<EventType>))]
    public record EventType(string Value) : EnumRecord(Value)
    {
        public static EventType ProvisioningToSmsPlatform = new("PROVISIONING_TO_SMS_PLATFORM");
        public static EventType DeprovisioningFromSmsPlatform = new("DEPROVISIONING_FROM_SMS_PLATFORM");
        public static EventType ProvisioningToCampaign = new("PROVISIONING_TO_CAMPAIGN");
        public static EventType DeprovisioningFromCampaignProvisioningToCampaign = new("DEPROVISIONING_FROM_CAMPAIGN");
        public static EventType ProvisioningToVoicePlatform = new("PROVISIONING_TO_VOICE_PLATFORM");
        public static EventType DeprovisioningToVoicePlatform = new("DEPROVISIONING_TO_VOICE_PLATFORM");
    }
}
