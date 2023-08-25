using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    public class CarouselMessage : MessageBase
    {
        /// <summary>
        ///     A list of up to 10 cards.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<CardMessage> Cards { get; set; }
#else
        public List<CardMessage> Cards { get; set; }
#endif


        /// <summary>
        ///     Optional. Outer choices on the carousel level. The number of outer choices is limited to 3.
        /// </summary>
        public List<Choice> Choices { get; set; }


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
