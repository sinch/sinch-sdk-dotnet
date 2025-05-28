using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(EnumRecordJsonConverter<EventType>))]
    public record EventType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     An event that occurs when a number is linked to a Service Plan ID.
        /// </summary>
        public static EventType ProvisioningToSmsPlatform = new("PROVISIONING_TO_SMS_PLATFORM");

        /// <summary>
        ///      An event that occurs when a number is unlinked from a Service Plan ID.
        /// </summary>
        public static EventType DeprovisioningFromSmsPlatform = new("DEPROVISIONING_FROM_SMS_PLATFORM");

        /// <summary>
        ///     An event that occurs when a number is linked to a Campaign.
        /// </summary>
        public static EventType ProvisioningToCampaign = new("PROVISIONING_TO_CAMPAIGN");

        /// <summary>
        ///     An event that occurs when a number is unlinked from a Campaign.
        /// </summary>
        public static EventType DeprovisioningFromCampaignProvisioningToCampaign = new("DEPROVISIONING_FROM_CAMPAIGN");

        /// <summary>
        ///     An event that occurs when a number is enabled for Voice operations.
        /// </summary>
        public static EventType ProvisioningToVoicePlatform = new("PROVISIONING_TO_VOICE_PLATFORM");

        /// <summary>
        ///     An event that occurs when a number is disabled for Voice operations.
        /// </summary>
        public static EventType DeprovisioningToVoicePlatform = new("DEPROVISIONING_TO_VOICE_PLATFORM");
    }
}
