using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     Binary MO (Mobile Originated) - an inbound binary SMS message.
    /// </summary>
    /// <seealso href="https://community.sinch.com/t5/Glossary/Binary-SMS/ta-p/7470">Binary SMS</seealso>
    /// <seealso href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/">SMS Webhooks Documentation</seealso>
    public sealed class BinaryMessage : IInboundMessage
    {
        /// <summary>
        ///     The message content Base64 encoded. Max 140 bytes together with udh.
        /// </summary>
        [JsonPropertyName("body")]
        public required string Body { get; set; }

        /// <summary>
        ///     If this inbound message is in response to a previously sent message that contained a client reference,
        ///     then this field contains that client reference.
        ///     Utilizing this feature requires additional setup on your account.
        ///     Contact your <see href="https://dashboard.sinch.com/settings/account-details">account manager</see> to enable this feature.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        /// <summary>
        ///     The phone number that sent the message.
        ///     <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">More info</see>
        /// </summary>
        [JsonPropertyName("from")]
        public required string From { get; set; }

        /// <summary>
        ///     The ID of this inbound message.
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        /// <summary>
        ///     The MCC/MNC of the sender's operator if known.
        /// </summary>
        [JsonPropertyName("operator_id")]
        public string? OperatorId { get; set; }

        /// <summary>
        ///     When the system received the message.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("received_at")]
        public required DateTime ReceivedAt { get; set; }

        /// <summary>
        ///     When the message left the originating device. Only available if provided by operator.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("sent_at")]
        public DateTime SentAt { get; set; }

        /// <summary>
        ///     The Sinch phone number or short code to which the message was sent.
        /// </summary>
        [JsonPropertyName("to")]
        public required string To { get; set; }

        /// <summary>
        ///     Gets or sets the Type. SMS in binary format.
        /// </summary>
        [JsonPropertyName("type")]
        public required Sinch.SMS.Inbounds.SmsType Type { get; set; }

        /// <summary>
        ///     The UDH header of a binary message HEX encoded. Max 140 bytes together with body.
        /// </summary>
        [JsonPropertyName("udh")]
        public required string Udh { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object.
        /// </summary>
        /// <returns>String presentation of the object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(BinaryMessage)} {{\n");
            sb.Append("  Body: ").Append(Body).Append('\n');
            sb.Append("  ClientReference: ").Append(ClientReference).Append('\n');
            sb.Append("  From: ").Append(From).Append('\n');
            sb.Append("  Id: ").Append(Id).Append('\n');
            sb.Append("  OperatorId: ").Append(OperatorId).Append('\n');
            sb.Append("  ReceivedAt: ").Append(ReceivedAt).Append('\n');
            sb.Append("  SentAt: ").Append(SentAt).Append('\n');
            sb.Append("  To: ").Append(To).Append('\n');
            sb.Append("  Type: ").Append(Type).Append('\n');
            sb.Append("  Udh: ").Append(Udh).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
