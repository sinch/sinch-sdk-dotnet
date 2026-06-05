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
    public sealed class UrlMessage
    {
        [JsonConstructor]
        public UrlMessage()
        {
        }

        [Obsolete(
            message:
            "This method is obsolete and will be removed in a future version. Consider initializing properties directly",
            error: false)]
        public UrlMessage(string title, Uri url)
        {
            Title = title;
            Url = url.ToString();
        }

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
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif

        /// <summary>
        ///     The timestamp defines start of a calendar event.
        /// </summary>
        [JsonPropertyName("event_start")]
#if NET7_0_OR_GREATER
        public required DateTime EventStart { get; set; }
#else
        public DateTime EventStart { get; set; }
#endif

        /// <summary>
        ///     The timestamp defines end of a calendar event.
        /// </summary>
        [JsonPropertyName("event_end")]
#if NET7_0_OR_GREATER
        public required DateTime EventEnd { get; set; }
#else
        public DateTime EventEnd { get; set; }
#endif

        /// <summary>
        ///     Title of a calendar event.
        /// </summary>
        [JsonPropertyName("event_title")]
#if NET7_0_OR_GREATER
        public required string EventTitle { get; set; }
#else
        public string EventTitle { get; set; } = null!;
#endif

        /// <summary>
        ///     Description of a calendar event.
        /// </summary>
        [JsonPropertyName("event_description")]
        public string? EventDescription { get; set; }

        /// <summary>
        ///     The URL that is opened when the user cannot open a calendar event directly or channel does not have support for this type.
        /// </summary>
        [JsonPropertyName("fallback_url")]
#if NET7_0_OR_GREATER
        public required string FallbackUrl { get; set; }
#else
        public string FallbackUrl { get; set; } = null!;
#endif
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
        ///     Create an instance of CallMessage
        /// </summary>
        /// <param name="phoneNumber">Phone number in E.164 with leading +.</param>
        /// <param name="title">Title shown close to the phone number. The title is clickable in some cases.</param>
        [Obsolete(
            message:
            "This method is obsolete and will be removed in a future version. Consider initializing properties directly",
            error: false)]
        public CallMessage(string phoneNumber, string title)
        {
            PhoneNumber = phoneNumber;
            Title = title;
        }

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
