using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///         An inbound message is a message sent to one of your short codes or long numbers from a mobile phone.
    ///         To receive inbound message callbacks, a URL needs to be added to your REST API.
    ///         This URL can be specified in your <see href="https://dashboard.sinch.com/sms/api">Dashboard</see>.
    /// </summary>
    public class IncomingTextSms : IIncomingSms
    {
        /// <summary>
        ///     Gets or Sets Body
        /// </summary>
        [JsonPropertyName("body")]
#if NET7_0_OR_GREATER
    public required virtual string Body { get; set; }
#else
        public virtual string Body { get; set; } = null!;
#endif


        /// <summary>
        ///     If this inbound message is in response to a previously sent message that contained a client reference,
        ///     then this field contains *that* client reference.
        ///     Utilizing this feature requires additional setup on your account.
        ///     Contact your
        ///     <see href="">https://dashboard.sinch.com/settings/account-details</see> to enable this feature.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }


        /// <summary>
        /// The phone number that sent the message.
        /// </summary>
        /// <value>The phone number that sent the message.</value>
        /// <example>+11203494390</example>
        [JsonPropertyName("from")]
#if NET7_0_OR_GREATER
        public required string From { get; set; }
#else
        public string From { get; set; } = null!;
#endif


        /// <summary>
        /// The ID of this inbound message.
        /// </summary>
        /// <value>The ID of this inbound message.</value>
        /// <example>01FC66621XXXXX119Z8PMV1QPA</example>
        [JsonPropertyName("id")]
#if NET7_0_OR_GREATER
        public required string Id { get; set; }
#else
        public string Id { get; set; } = null!;
#endif


        /// <summary>
        /// The MCC/MNC of the sender's operator if known.
        /// </summary>
        /// <value>The MCC/MNC of the sender's operator if known.</value>
        /// <example>35000</example>
        [JsonPropertyName("operator_id")]
        public string? OperatorId { get; set; }


        /// <summary>
        ///     When the system received the message.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("received_at")]
#if NET7_0_OR_GREATER
        public required DateTime ReceivedAt { get; set; }
#else
        public DateTime ReceivedAt { get; set; }
#endif


        /// <summary>
        ///     When the message left the originating device. Only available if provided by operator.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("sent_at")]
        public DateTime SentAt { get; set; }


        /// <summary>
        ///     The Sinch phone number or short code to which the message was sent.
        /// </summary>
        /// <example>
        ///     11203453453
        /// </example>
        [JsonPropertyName("to")]
#if NET7_0_OR_GREATER
        public required string To { get; set; }
#else
        public string To { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets Type
        /// </summary>
        [JsonPropertyName("type")]
#if NET7_0_OR_GREATER
        public required Sinch.SMS.Inbounds.SmsType Type { get; set; }
#else
        public Inbounds.SmsType Type { get; set; } = null!;
#endif


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiMoMessage {\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  ClientReference: ").Append(ClientReference).Append("\n");
            sb.Append("  From: ").Append(From).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OperatorId: ").Append(OperatorId).Append("\n");
            sb.Append("  ReceivedAt: ").Append(ReceivedAt).Append("\n");
            sb.Append("  SentAt: ").Append(SentAt).Append("\n");
            sb.Append("  To: ").Append(To).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
