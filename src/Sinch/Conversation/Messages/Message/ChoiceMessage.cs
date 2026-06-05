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
        public required List<Choice> Choices { get; set; }

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
    public sealed class UrlMessage
    {
        [JsonConstructor]
        public UrlMessage()
        {
        }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }

    /// <summary>
    ///     Message containing details about a calendar event.
    /// </summary>
    public sealed class CalendarMessage
    {
        [JsonConstructor]
        public CalendarMessage()
        {
        }

        /// <summary>
        ///     The title is shown close to the button that leads to open a user calendar.
        /// </summary>
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        /// <summary>
        ///     The timestamp defines start of a calendar event.
        /// </summary>
        [JsonPropertyName("event_start")]
        public required DateTime EventStart { get; set; }

        /// <summary>
        ///     The timestamp defines end of a calendar event.
        /// </summary>
        [JsonPropertyName("event_end")]
        public required DateTime EventEnd { get; set; }

        /// <summary>
        ///     Title of a calendar event.
        /// </summary>
        [JsonPropertyName("event_title")]
        public required string EventTitle { get; set; }

        /// <summary>
        ///     Description of a calendar event.
        /// </summary>
        [JsonPropertyName("event_description")]
        public string? EventDescription { get; set; }

        /// <summary>
        ///     The URL that is opened when the user cannot open a calendar event directly or channel does not have support for this type.
        /// </summary>
        [JsonPropertyName("fallback_url")]
        public required string FallbackUrl { get; set; }

    }

    /// <summary>
    ///     Message requesting location from a user.
    /// </summary>
    public sealed class ShareLocationMessage
    {
        [JsonConstructor]
        public ShareLocationMessage()
        {
        }

        /// <summary>
        ///     The title is shown close to the button that leads to open a map to share a location.
        /// </summary>
        [JsonPropertyName("title")]
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif

        /// <summary>
        ///     The URL that is opened when channel does not have support for this type.
        /// </summary>
        [JsonPropertyName("fallback_url")]
#if NET7_0_OR_GREATER
        public required string FallbackUrl { get; set; }
#else
        public string FallbackUrl { get; set; } = null!;
#endif
    }

    /// <summary>
    ///     Message for triggering a call.
    /// </summary>
    public sealed class CallMessage
    {
        [JsonConstructor]
        public CallMessage()
        {
        }

        /// <summary>
        ///     Phone number in E.164 with leading +.
        /// </summary>
        [JsonPropertyName("phone_number")]
        public required string PhoneNumber { get; set; }

        /// <summary>
        ///     Title shown close to the phone number. The title is clickable in some cases.
        /// </summary>
        [JsonPropertyName("title")]
        public required string Title { get; set; }
    }
}
