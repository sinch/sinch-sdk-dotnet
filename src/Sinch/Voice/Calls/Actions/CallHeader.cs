using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Call Headers can be used to pass custom data from a Sinch SDK client to another, or specified in an ICE response to
    ///     be made available to the receiving client. Further, if Call Headers is specified they will be available in ICE and
    ///     DICE events.
    /// </summary>
    public sealed class CallHeader
    {
        /// <summary>
        ///     The call header key of the key value pair.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }


        /// <summary>
        ///     The call header value of the key value pair.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CallHeader {\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
