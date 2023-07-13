using System;

namespace Sinch.SMS.Inbounds
{
    public class Inbound
    {
        /// <summary>
        ///     The ID of this inbound message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The object type.
        /// </summary>
        public SmsType Type { get; set; }

        /// <summary>
        ///     The phone number that sent the message.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     The Sinch phone number or short code to which the message was sent.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        ///     The message body. Base64 encoded if type is mo_binary.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     If this inbound message is in response to a previously sent message that contained a client reference,
        ///     then this field contains that client reference.<br /><br />
        ///     Utilizing this feature requires additional setup on your account.
        ///     Contact your <see href="https://dashboard.sinch.com/settings/account-details">account manager</see>
        ///     to enable this feature.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     The MCC/MNC of the sender's operator if known.
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        ///     When the message left the originating device. Only available if provided by operator.
        /// </summary>
        public DateTime SendAt { get; set; }

        /// <summary>
        ///     When the system received the message.
        /// </summary>
        public DateTime ReceivedAt { get; set; }
    }
}
