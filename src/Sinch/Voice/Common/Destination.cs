using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Common
{
    /// <summary>
    ///     The type of device and number or endpoint to call.
    /// </summary>
    public sealed class Destination
    {

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
#if NET7_0_OR_GREATER
        public required DestinationType Type { get; set; }
#else 
        public DestinationType Type { get; set; } = null!;
#endif

        /// <summary>
        ///     If the type is &#x60;number&#x60; the value of the endpoint is a phone number. If the type is &#x60;username&#x60; the value is the username for a data endpoint.
        /// </summary>
        [JsonPropertyName("endpoint")]
#if NET7_0_OR_GREATER
        public required string Endpoint { get; set; }
#else
        public string Endpoint { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Destination)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Endpoint)}: ").Append(Endpoint).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

    /// <summary>
    /// Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.
    /// </summary>
    /// <value>Can be of type &#x60;number&#x60; for PSTN endpoints or of type &#x60;username&#x60; for data endpoints.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<DestinationType>))]
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
