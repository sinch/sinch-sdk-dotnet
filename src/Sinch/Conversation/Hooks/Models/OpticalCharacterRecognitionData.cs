using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     OpticalCharacterRecognitionData
    /// </summary>
    public sealed class OpticalCharacterRecognitionData
    {
        /// <summary>
        ///     The data array contains the string(s) identified in one section of an analyzed image.
        /// </summary>
        [JsonPropertyName("data")]
        public List<string> Data { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OpticalCharacterRecognitionData)} {{\n");
            sb.Append($"  {nameof(Data)}: ").Append(Data).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
