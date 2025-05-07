using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Common
{
    /// <summary>
    /// Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.
    /// </summary>
    /// <value>Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.</value>
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
    }

    /// <summary>
    /// Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.
    /// </summary>
    /// <value>Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.</value>
    [JsonConverter(typeof(EnumRecordCaseInsensitiveJsonConverter<DestinationTypeExtended>))]
    public record DestinationTypeExtended(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Destination ptsn
        /// </summary>
        public static readonly DestinationTypeExtended Number = new("number");
        /// <summary>
        ///     Destination mxp
        /// </summary>
        public static readonly DestinationTypeExtended Username = new("username");
        public static readonly DestinationTypeExtended Sip = new("sip");
        public static readonly DestinationTypeExtended Did = new("did");
    }
}
