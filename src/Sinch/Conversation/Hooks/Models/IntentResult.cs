using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     IntentResult
    /// </summary>
    public sealed class IntentResult
    {
        /// <summary>
        ///     The most probable intent of the analyzed text. For example, chitchat.greeting, chitchat.bye, chitchat.compliment, chitchat.how_are_you, or general.yes_or_agreed.
        /// </summary>
        [JsonPropertyName("intent")]
        public string Intent { get; set; }


        /// <summary>
        ///     The likelihood that the assigned intent represents the purpose of the analyzed text. 1 is the maximum value, representing the highest likelihood that the message text matches the intent, and 0 is the minimum value, representing the lowest likelihood that the message text matches the intent.
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
            sb.Append($"class {nameof(IntentResult)} {{\n");
            sb.Append($"  {nameof(Intent)}: ").Append(Intent).Append('\n');
            sb.Append($"  {nameof(Score)}: ").Append(Score).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
