using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Applications.QueryNumber
{
    public sealed class QueryNumberResponse
    {
        /// <summary>
        ///     The type of method.
        /// </summary>
        public string? Method { get; set; }


        /// <summary>
        ///     Gets or Sets Number
        /// </summary>
        [JsonPropertyName("number")]
        // TODO: rename to Number in 2.0
        public NumberItem? NumberItem { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class QueryNumberResponse {\n");
            sb.Append("  Method: ").Append(Method).Append("\n");
            sb.Append("  Number: ").Append(NumberItem).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
