using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Message containing text, media and choices.
    /// </summary>
    public sealed class CardMessage : IOmniMessageOverride
    {
        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [JsonPropertyName("height")]
        public CardHeight? Height { get; set; }

        /// <summary>
        ///     You may include choices in your Card Message. The number of choices is limited to 10.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; set; }


        /// <summary>
        ///     This is an optional description field that is displayed below the title on the card.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Gets or Sets MediaMessage
        /// </summary>
        [JsonPropertyName("media_message")]
        public MediaProperties? MediaMessage { get; set; }


        /// <summary>
        ///     The title of the card message.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CardMessage {\n");
            sb.Append("  Choices: ").Append(Choices).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  MediaMessage: ").Append(MediaMessage).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     A message containing an image media component.
    /// </summary>
    public sealed class CardMessageMediaMessage
    {
        /// <summary>
        ///     Url to the media file.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
