using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.SinchEvents
{
    /// <summary>
    ///     Discriminator type for Fax Sinch events.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<FaxEventType>))]
    public record FaxEventType(string Value) : EnumRecord(Value)
    {
        public static readonly FaxEventType IncomingFax = new("INCOMING_FAX");
        public static readonly FaxEventType CompletedFax = new("FAX_COMPLETED");
    }
}
