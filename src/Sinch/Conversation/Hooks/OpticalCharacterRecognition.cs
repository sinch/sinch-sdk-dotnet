using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     An object containing a result array that reports the machine learning engine&#39;s character extraction results.
    /// </summary>
    public sealed class OpticalCharacterRecognition
    {
        /// <summary>
        ///     The result of the OCR process.
        /// </summary>
        [JsonPropertyName("result")]
        public List<OpticalCharacterRecognitionData> Result { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OpticalCharacterRecognition)} {{\n");
            sb.Append($"  {nameof(Result)}: ").Append(Result).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
