using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    /// <summary>
    ///     Represents the types of numbers.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Types>))]
    public record Types(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Numbers that belong to a specific range (e.g., mobile numbers).
        /// </summary>
        public static readonly Types Mobile = new("MOBILE");

        /// <summary>
        ///     Numbers that are assigned to a specific geographic region (e.g., local numbers).
        /// </summary>
        public static readonly Types Local = new("LOCAL");

        /// <summary>
        ///     Numbers that are free of charge for the calling party but billed for all arriving calls (e.g., toll-free numbers).
        /// </summary>
        public static readonly Types TollFree = new("TOLL_FREE");
    }
}
