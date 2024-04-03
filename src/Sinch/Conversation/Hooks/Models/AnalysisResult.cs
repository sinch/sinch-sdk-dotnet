using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     The analysis provided by the Smart Conversations machine learning engine(s). The contents of the object are determined by the functionalities that are enabled for your solution.
    /// </summary>
    public sealed class AnalysisResult
    {
        /// <summary>
        ///     An array that contains the analyses of the sentiments of the corresponding messages.
        /// </summary>
        [JsonPropertyName("ml_sentiment_result")]
        public List<MachineLearningSentimentResult> MlSentimentResult { get; set; }
        

        /// <summary>
        ///     An array that contains the analyses of the intentions of, and entities within, the corresponding messages.
        /// </summary>
        [JsonPropertyName("ml_nlu_result")]
        public List<MachineLearningNLUResult> MlNluResult { get; set; }
        

        /// <summary>
        ///     An array that contains the image recognition analyses of the images identified in the corresponding messages.
        /// </summary>
        [JsonPropertyName("ml_image_recognition_result")]
        public List<MachineLearningImageRecognitionResult> MlImageRecognitionResult { get; set; }
        

        /// <summary>
        ///     An array that contains the PII analysis of the corresponding messages.
        /// </summary>
        [JsonPropertyName("ml_pii_result")]
        public List<MachineLearningPIIResult> MlPiiResult { get; set; }
        

        /// <summary>
        ///     An array that contains the analyses of the offenses of the corresponding messages.
        /// </summary>
        [JsonPropertyName("ml_offensive_analysis_result")]
        public List<OffensiveAnalysis> MlOffensiveAnalysisResult { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(AnalysisResult)} {{\n");
            sb.Append($"  {nameof(MlSentimentResult)}: ").Append(MlSentimentResult).Append('\n');
            sb.Append($"  {nameof(MlNluResult)}: ").Append(MlNluResult).Append('\n');
            sb.Append($"  {nameof(MlImageRecognitionResult)}: ").Append(MlImageRecognitionResult).Append('\n');
            sb.Append($"  {nameof(MlPiiResult)}: ").Append(MlPiiResult).Append('\n');
            sb.Append($"  {nameof(MlOffensiveAnalysisResult)}: ").Append(MlOffensiveAnalysisResult).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
