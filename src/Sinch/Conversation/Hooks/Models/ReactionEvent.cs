using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    public class ReactionEvent
    {
        /// <summary>
        ///     Indicates that an emoji reaction was placed on a message. This value is the string representation of the emoji. For example: "\u{2764}\u{FE0F}"
        /// </summary>
        [JsonPropertyName("emoji")]
        public string? Emoji { get; set; }

        /// <summary>
        ///     Type of action.
        /// </summary>
        [JsonPropertyName("action")]
        public ReactionAction? Action { get; set; }

        /// <summary>
        ///     The ID of the MT message that this reaction is associated with.
        /// </summary>
        [JsonPropertyName("message_id")]
        public string? MessageId { get; set; }

        /// <summary>
        ///     If present, represents the grouping of emojis. Example values: "smile, "angry, "sad", "wow, "love", "like", "dislike", "other"
        /// </summary>
        [JsonPropertyName("reaction_category")]
        public string? ReactionCategory { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<ReactionAction>))]
    public record ReactionAction(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Unrecognized type of action
        /// </summary>
        public static readonly ReactionAction Unknown = new("REACTION_ACTION_UNKNOWN");

        /// <summary>
        ///     User placed some emoji reaction
        /// </summary>
        public static readonly ReactionAction React = new("REACTION_ACTION_REACT");

        /// <summary>
        ///     User removed previously placed emoji reaction
        /// </summary>
        public static readonly ReactionAction Unreact = new("REACTION_ACTION_UNREACT");
    }
}
