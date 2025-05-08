using System.Text;
using System.Text.Json.Serialization;

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
        public required ParticipantType Type { get; set; }
#else 
        public ParticipantType Type { get; set; } = null!;
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
}
