using System;
using System.Collections.Generic;

namespace Sinch.SMS.Batches.Update
{
    public class Request
    {
        /// <summary>
        ///     Sender number. Must be valid phone number, short code or alphanumeric.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     The message content. Normal text string for mt_text and Base64 encoded for mt_binary
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     List of phone numbers and group IDs to add to the batch.
        /// </summary>
        public List<string> ToAdd { get; set; } = new();

        /// <summary>
        ///     List of phone numbers and group IDs to remove from the batch.
        /// </summary>
        public List<string> ToRemove { get; set; } = new();

        /// <summary>
        ///     Request delivery report callback. Note that delivery reports can be fetched from the API regardless of this
        ///     setting.
        /// </summary>
        public DeliveryReport? DeliveryReport { get; set; }

        /// <summary>
        ///     If set, in the future the message will be delayed until send_at occurs.
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        ///     If set, the system will stop trying to deliver the message at this point.
        ///     <br /><br />
        ///     Must be after <see cref="SendAt" />
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        ///     Override the default callback URL for this batch.
        /// </summary>
        public Uri CallbackUrl { get; set; }
    }
}
