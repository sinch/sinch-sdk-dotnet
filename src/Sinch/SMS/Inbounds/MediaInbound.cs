using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;
using System.Collections.Generic;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     MOMedia
    /// </summary>
    public sealed class MediaInbound : IInbound
    {
        /// <summary>
        ///     If this inbound message is in response to a previously sent message that contained a client reference, then this field contains *that* client reference.   Utilizing this feature requires additional setup on your account. Contact your [account manager](https://dashboard.sinch.com/settings/account-details) to enable this feature.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }


        /// <summary>
        ///     The phone number that sent the message. [More info](https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628)
        /// </summary>
        [JsonPropertyName("from")]
#if NET7_0_OR_GREATER
        public required string From { get; set; }
#else
        public string From { get; set; } = null!;
#endif


        /// <summary>
        ///     The ID of this inbound message.
        /// </summary>
        [JsonPropertyName("id")]
#if NET7_0_OR_GREATER
        public required string Id { get; set; }
#else
        public string Id { get; set; } = null!;
#endif


        /// <summary>
        ///     The MCC/MNC of the sender&#39;s operator if known.
        /// </summary>
        [JsonPropertyName("operator_id")]
        public string? OperatorId { get; set; }


        /// <summary>
        ///     When the system received the message.   Formatted as [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601): &#x60;YYYY-MM-DDThh:mm:ss.SSSZ&#x60;.
        /// </summary>
        [JsonPropertyName("received_at")]
#if NET7_0_OR_GREATER
        public required DateTime ReceivedAt { get; set; }
#else
        public DateTime ReceivedAt { get; set; }
#endif


        /// <summary>
        ///     When the message left the originating device. Only available if provided by operator.  Formatted as [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601): &#x60;YYYY-MM-DDThh:mm:ss.SSSZ&#x60;.
        /// </summary>
        [JsonPropertyName("sent_at")]
        public DateTime? SentAt { get; set; }


        /// <summary>
        ///     The Sinch phone number or short code to which the message was sent.
        /// </summary>
        [JsonPropertyName("to")]
#if NET7_0_OR_GREATER
        public required string To { get; set; }
#else
        public string To { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets Body
        /// </summary>
        [JsonPropertyName("body")]
#if NET7_0_OR_GREATER
        public required MmsMoBody Body { get; set; }
#else
        public MmsMoBody Body { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MediaInbound)} {{\n");
            sb.Append($"  {nameof(ClientReference)}: ").Append(ClientReference).Append('\n');
            sb.Append($"  {nameof(From)}: ").Append(From).Append('\n');
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(OperatorId)}: ").Append(OperatorId).Append('\n');
            sb.Append($"  {nameof(ReceivedAt)}: ").Append(ReceivedAt).Append('\n');
            sb.Append($"  {nameof(SentAt)}: ").Append(SentAt).Append('\n');
            sb.Append($"  {nameof(To)}: ").Append(To).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public sealed class MmsMoBody
    {
        /// <summary>
        ///     The subject of the MMS media message.
        /// </summary>
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }


        /// <summary>
        ///     The text message content of the MMS media message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }


        /// <summary>
        ///     Collection of attachments in incoming message.
        /// </summary>
        [JsonPropertyName("media")]
        public List<MmsMedia>? Media { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MmsMoBody)} {{\n");
            sb.Append($"  {nameof(Subject)}: ").Append(Subject).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(Media)}: ").Append(Media).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Collection of attachments in incoming message.
    /// </summary>
    public sealed class MmsMedia
    {
        /// <summary>
        /// Status of the uploaded media.
        /// </summary>
        /// <value>Status of the uploaded media.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<StatusEnum>))]
        public record StatusEnum(string Value) : EnumRecord(Value)
        {
            public static readonly StatusEnum Uploaded = new("Uploaded");
            public static readonly StatusEnum Failed = new("Failed");
        }


        /// <summary>
        /// Status of the uploaded media.
        /// </summary>
        [JsonPropertyName("status")]
#if NET7_0_OR_GREATER
        public required StatusEnum Status { get; set; }
#else
        public StatusEnum Status { get; set; } = null!;
#endif

        /// <summary>
        ///     The result code. Possible values: 0 (success), 1 (content upload error), 2 (cloud bucket error), 3 (bucket key error).
        /// </summary>
        [JsonPropertyName("code")]
#if NET7_0_OR_GREATER
        public required int Code { get; set; }
#else
        public int Code { get; set; }
#endif


        /// <summary>
        ///     Content type of binary. [More info](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types)
        /// </summary>
        [JsonPropertyName("content_type")]
#if NET7_0_OR_GREATER
        public required string ContentType { get; set; }
#else
        public string ContentType { get; set; } = null!;
#endif


        /// <summary>
        ///     URL to be used to download attachment.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MmsMedia)} {{\n");
            sb.Append($"  {nameof(Code)}: ").Append(Code).Append('\n');
            sb.Append($"  {nameof(ContentType)}: ").Append(ContentType).Append('\n');
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
