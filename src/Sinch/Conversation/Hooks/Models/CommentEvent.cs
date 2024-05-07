using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     Object which contains information of a comment made by an user outside of the main conversation context. Currently only supported on Instagram channel, see Instagram Private Replies for more details
    /// </summary>
    public sealed class CommentEvent
    {
        /// <summary>
        ///     Either LIVE or FEED. Indicates the type of media on which the comment was made.
        /// </summary>
        [JsonPropertyName("comment_type")]
        public CommentType? CommentType { get; set; }

        /// <summary>
        ///     Event&#39;s ID
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }


        /// <summary>
        ///     Comment&#39;s text
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }


        /// <summary>
        ///     Instagram&#39;s URL of the live broadcast or the post on which the comment was made (permalink).
        /// </summary>
        [JsonPropertyName("commented_on")]
        public string? CommentedOn { get; set; }


        /// <summary>
        ///     Username of the account that commented in the live broadcast or post.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(CommentEvent)} {{\n");
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Text)}: ").Append(Text).Append('\n');
            sb.Append($"  {nameof(CommentType)}: ").Append(CommentType).Append('\n');
            sb.Append($"  {nameof(CommentedOn)}: ").Append(CommentedOn).Append('\n');
            sb.Append($"  {nameof(User)}: ").Append(User).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Either LIVE or FEED. Indicates the type of media on which the comment was made.
    /// </summary>
    /// <value>Either LIVE or FEED. Indicates the type of media on which the comment was made.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CommentType>))]
    public record CommentType(string Value) : EnumRecord(Value)
    {
        public static readonly CommentType Feed = new("FEED");
        public static readonly CommentType Live = new("LIVE");
    }
}
