using System;

namespace Sinch.SMS.Inbounds
{
    public class SmsInbound : IInbound
    {
        /// <summary>
        ///     The ID of this inbound message.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Id { get; set; }

#else
        public string Id { get; set; } = null!;

#endif

        /// <summary>
        ///     The phone number that sent the message.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string From { get; set; }
#else
        public string From { get; set; } = null!;
#endif


        /// <summary>
        ///     The Sinch phone number or short code to which the message was sent.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string To { get; set; }
#else
        public string To { get; set; } = null!;
#endif

#if NET7_0_OR_GREATER
        public required string Body { get; set; }
#else
        public string Body { get; set; } = null!;
#endif


        /// <summary>
        ///     If this inbound message is in response to a previously sent message that contained a client reference,
        ///     then this field contains that client reference.<br /><br />
        ///     Utilizing this feature requires additional setup on your account.
        ///     Contact your <see href="https://dashboard.sinch.com/settings/account-details">account manager</see>
        ///     to enable this feature.
        /// </summary>
        public string? ClientReference { get; set; }

        /// <summary>
        ///     The MCC/MNC of the sender's operator if known.
        /// </summary>
        public string? OperatorId { get; set; }

        /// <summary>
        ///     When the message left the originating device. Only available if provided by operator.
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        ///     When the system received the message.
        /// </summary>
#if NET7_0_OR_GREATER
        public required DateTime ReceivedAt { get; set; }
#else
        public DateTime ReceivedAt { get; set; }
#endif
    }
}
