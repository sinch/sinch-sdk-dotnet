using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    /// <summary>
    ///     Represents the event status options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<EventStatus>))]
    public record EventStatus(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The event succeeded.
        /// </summary>
        public static readonly EventStatus Succeeded = new("SUCCEEDED");

        /// <summary>
        ///     The event failed.
        /// </summary>
        public static readonly EventStatus Failed = new("FAILED");
    }
}
