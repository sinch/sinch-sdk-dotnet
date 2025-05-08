using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     Known destination, including `did`.
    /// </summary>
    [JsonConverter(typeof(EnumRecordCaseInsensitiveJsonConverter<DestinationType>))]
    public record DestinationType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Destination ptsn
        /// </summary>
        public static readonly DestinationType Number = new("number");
        /// <summary>
        ///     Destination mxp
        /// </summary>
        public static readonly DestinationType Username = new("username");
        public static readonly DestinationType Sip = new("sip");
        public static readonly DestinationType Did = new("did");
    }
}
