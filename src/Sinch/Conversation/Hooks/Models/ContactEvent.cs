using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     ContactEvent
    /// </summary>
    public sealed class ContactEvent
    {
        /// <summary>
        ///     Empty object denoting the contact is composing a message.
        /// </summary>
        [JsonPropertyName("composing_event")]
        public object ComposingEvent { get; set; }


        /// <summary>
        ///     Gets or Sets CommentEvent
        /// </summary>
        [JsonPropertyName("comment_event")]
        public CommentEvent CommentEvent { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactEvent)} {{\n");
            sb.Append($"  {nameof(ComposingEvent)}: ").Append(ComposingEvent).Append('\n');
            sb.Append($"  {nameof(CommentEvent)}: ").Append(CommentEvent).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
