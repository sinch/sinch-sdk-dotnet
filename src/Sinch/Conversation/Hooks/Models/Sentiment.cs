using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     The most probable sentiment of the analyzed text.
    /// </summary>
    /// <value>The most probable sentiment of the analyzed text.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<Sentiment>))]
    public record Sentiment(string Value) : EnumRecord(Value)
    {
        public static readonly Sentiment Positive = new("positive");
        public static readonly Sentiment Negative = new("negative");
        public static readonly Sentiment Neutral = new("neutral");
    }
}
