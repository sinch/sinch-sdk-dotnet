using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(EnumRecordJsonConverter<EventType>))]
    public record EventType(string Value) : EnumRecord(Value)
    {
        public static EventType ProvisioningToSmsPlatform = new("PROVISIONING_TO_SMS_PLATFORM");
        public static EventType LinkTo10DlcCampaign = new("LINK_TO_10DLC_CAMPAIGN");
    }
}
