using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public sealed class CarouselMessage : IOmniMessageOverride
    {
        /// <summary>
        ///     A list of up to 10 cards.
        /// </summary>
        [JsonPropertyName("cards")]
        public required List<CardMessage> Cards { get; set; }


        /// <summary>
        ///     Optional. Outer choices on the carousel level. The number of outer choices is limited to 3.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CarouselMessage {\n");
            sb.Append("  Cards: ").Append(Cards).Append("\n");
            sb.Append("  Choices: ").Append(Choices).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
