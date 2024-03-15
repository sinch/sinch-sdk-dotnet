using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     SentimentResult
    /// </summary>
    public sealed class SentimentResult
    {
        /// <summary>
        /// The most probable sentiment of the analyzed text.
        /// </summary>
        [JsonPropertyName("sentiment")]
        public Sentiment? Sentiment { get; set; }

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
            sb.Append($"class {nameof(SentimentResult)} {{\n");
            sb.Append($"  {nameof(Sentiment)}: ").Append(Sentiment).Append('\n');
            sb.Append($"  {nameof(Score)}: ").Append(Score).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
