using System.Text;
using System.Text.Json.Serialization;
using Sinch.Voice.Applications.GetNumbers;

namespace Sinch.Voice.Applications.UnassignNumbers
{
    public sealed class UnassignNumberRequest
    {
        /// <summary>
        ///     (optional) indicates the DID capability that was assigned to the chosen application. Please note that the DID needs
        ///     to support the selected capability.
        /// </summary>
        public Capability? Capability { get; set; }

        /// <summary>
        ///     The phone number in E.164 format (https://en.wikipedia.org/wiki/E.164)
        /// </summary>
        public string? Number { get; set; }


        /// <summary>
        ///     Indicates the application where the number(s) was assigned. If empty, the application key that is used to sign the
        ///     request will be used.
        /// </summary>
        [JsonPropertyName("applicationkey")]
        public string? ApplicationKey { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UnassignNumberRequest {\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("  Capability: ").Append(Capability).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
