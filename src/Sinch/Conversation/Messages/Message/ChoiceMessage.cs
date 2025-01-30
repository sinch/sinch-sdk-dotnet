using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public sealed class ChoiceMessage : IOmniMessageOverride
    {
        /// <summary>
        ///     The number of choices is limited to 10.
        /// </summary>
        [JsonPropertyName("choices")]
#if NET7_0_OR_GREATER
        public required List<Choice> Choices { get; set; }
#else
        public List<Choice> Choices { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        [JsonPropertyName("text_message")]
        public TextMessage? TextMessage { get; set; }


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
    public class UrlMessage
    {
        [JsonPropertyName("title")]
#if NET7_0_OR_GREATER
        public required string? Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif

        [JsonPropertyName("url")]
#if NET7_0_OR_GREATER
        public string? Url { get; set; }
#else
        public string Url { get; set; } = null!;
#endif
    }

    /// <summary>
    ///     Message for triggering a call.
    /// </summary>
    public sealed class CallMessage
    {
        /// <summary>
        ///     Phone number in E.164 with leading +.
        /// </summary>
        [JsonPropertyName("phone_number")]
#if NET7_0_OR_GREATER
public required  string PhoneNumber { get; set; }
#else
        public string PhoneNumber { get; set; } = null!;
#endif


        /// <summary>
        ///     Title shown close to the phone number. The title is clickable in some cases.
        /// </summary>
        [JsonPropertyName("title")]
#if NET7_0_OR_GREATER
         public required string Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif
    }
}
