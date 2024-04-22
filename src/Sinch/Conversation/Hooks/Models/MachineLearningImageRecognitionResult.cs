using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     MachineLearningImageRecognitionResult
    /// </summary>
    public sealed class MachineLearningImageRecognitionResult
    {
        /// <summary>
        ///     The URL of the image that was processed.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }


        /// <summary>
        ///     Gets or Sets DocumentImageClassification
        /// </summary>
        [JsonPropertyName("document_image_classification")]
        public DocumentImageClassification? DocumentImageClassification { get; set; }


        /// <summary>
        ///     Gets or Sets OpticalCharacterRecognition
        /// </summary>
        [JsonPropertyName("optical_character_recognition")]
        public OpticalCharacterRecognition? OpticalCharacterRecognition { get; set; }


        /// <summary>
        ///     Gets or Sets DocumentFieldClassification
        /// </summary>
        [JsonPropertyName("document_field_classification")]
        public DocumentFieldClassification? DocumentFieldClassification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MachineLearningImageRecognitionResult)} {{\n");
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append($"  {nameof(DocumentImageClassification)}: ").Append(DocumentImageClassification).Append('\n');
            sb.Append($"  {nameof(OpticalCharacterRecognition)}: ").Append(OpticalCharacterRecognition).Append('\n');
            sb.Append($"  {nameof(DocumentFieldClassification)}: ").Append(DocumentFieldClassification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
