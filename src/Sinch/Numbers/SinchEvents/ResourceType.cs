using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.SinchEvents
{
    /// <summary>
    ///     Represents the resource type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ResourceType>))]
    public record ResourceType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Numbers which are already active and updated with new campaign IDs or service plan IDs.
        /// </summary>
        public static readonly ResourceType ActiveNumber = new("ACTIVE_NUMBER");

        /// <summary>
        ///     Represents a hosting order resource.
        /// </summary>
        public static readonly ResourceType HostingOrder = new("HOSTING_ORDER");

        /// <summary>
        ///     Represents a brand resource.
        /// </summary>
        public static readonly ResourceType Brand = new("Brand");
    }
}
