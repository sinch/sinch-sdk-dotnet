using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    /// <summary>
    ///     Represents the provisioning status options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ProvisioningStatus>))]
    public record ProvisioningStatus(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The provisioning is waiting.
        /// </summary>
        public static readonly ProvisioningStatus Waiting = new("WAITING");

        /// <summary>
        ///     The provisioning is in progress.
        /// </summary>
        public static readonly ProvisioningStatus InProgress = new("IN_PROGRESS");

        /// <summary>
        ///     The provisioning has failed.
        /// </summary>
        public static readonly ProvisioningStatus Failed = new("FAILED");
    }
}
