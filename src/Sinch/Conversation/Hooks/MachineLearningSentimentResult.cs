using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     MachineLearningSentimentResult
    /// </summary>
    public sealed class MachineLearningSentimentResult
    {
        /// <summary>
        /// The most probable sentiment of the analyzed text.
        /// </summary>
        [JsonPropertyName("sentiment")]
        public Sentiment? Sentiment { get; set; }

        /// <summary>
        ///     The message text that was analyzed.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
        

        /// <summary>
        ///     An array of JSON objects made up of sentiment and score pairs, where the score represents the likelihood that the message communicates the corresponding sentiment.
        /// </summary>
        [JsonPropertyName("results")]
        public List<SentimentResult> Results { get; set; }
        

        /// <summary>
        ///     The likelihood that the assigned sentiment represents the emotional context of the analyzed text. 1 is the maximum value, representing the highest likelihood that the message text matches the sentiment, and 0 is the minimum value, representing the lowest likelihood that the message text matches the sentiment.
        /// </summary>
        [JsonPropertyName("score")]
        public float Score { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MachineLearningSentimentResult)} {{\n");
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(Results)}: ").Append(Results).Append('\n');
            sb.Append($"  {nameof(Sentiment)}: ").Append(Sentiment).Append('\n');
            sb.Append($"  {nameof(Score)}: ").Append(Score).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
