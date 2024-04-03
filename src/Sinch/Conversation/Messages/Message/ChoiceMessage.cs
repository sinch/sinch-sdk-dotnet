using System;
using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    public class ChoiceMessage
    {
        /// <summary>
        ///     The number of choices is limited to 10.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<Choice> Choices { get; set; }
#else
        public List<Choice> Choices { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        public TextMessage TextMessage { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ChoiceMessage {\n");
            sb.Append("  Choices: ").Append(Choices).Append("\n");
            sb.Append("  TextMessage: ").Append(TextMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     A generic URL message.
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Url"></param>
    public record UrlMessage(string Title, Uri Url);

    /// <summary>
    ///     Message for triggering a call.
    /// </summary>
    /// <param name="PhoneNumber">Phone number in E.164 with leading +.</param>
    /// <param name="Title">Title shown close to the phone number. The title is clickable in some cases.</param>
    public record CallMessage(string PhoneNumber, string Title);
}
