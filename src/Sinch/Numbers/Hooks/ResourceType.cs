using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    /// <summary>
    ///     Represents the resource type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ResourceType>))]
    public record ResourceType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a number resource.
        /// </summary>
        public static readonly ResourceType Number = new("NUMBER");

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
