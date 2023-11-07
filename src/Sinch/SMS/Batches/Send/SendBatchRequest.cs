﻿using System;
using System.Collections.Generic;

namespace Sinch.SMS.Batches.Send
{
    public class SendBatchRequest
    {
        /// <summary>
        ///     The message content. 2000 characters max
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Body { get; set; }
#else
        public string Body { get; set; }
#endif


        /// <summary>
        ///     Request delivery report callback. Note that delivery reports can be fetched from the API regardless of this
        ///     setting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required DeliveryReport DeliveryReport { get; set; }
#else
        public DeliveryReport DeliveryReport { get; set; }
#endif


        /// <summary>
        ///     List of Phone numbers and group IDs that will receive the batch.
        ///     <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">More info</see>
        /// </summary>
#if NET7_0_OR_GREATER
   public required List<string> To { get; set; }
#else
        public List<string> To { get; set; }
#endif


        /// <summary>
        ///     Sender number. Must be valid phone number, short code or alphanumeric.
        ///     Required if Automatic Default Originator not configured.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parameters { get; set; }

        /// <summary>
        ///     If set in the future, the message will be delayed until send_at occurs. Must be before expire_at.
        ///     If set in the past, messages will be sent immediately.
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        ///     If set, the system will stop trying to deliver the message at this point. Must be after send_at.
        ///     Default and max is 3 days after send_at.
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        ///     Override the default callback URL for this batch. Must be a valid URL.
        ///     Learn how to set a default callback URL
        ///     <see href="https://community.sinch.com/t5/SMS/How-do-I-assign-a-callback-URL-to-an-SMS-service-plan/ta-p/8414">here</see>
        ///     .
        /// </summary>
        public Uri CallbackUrl { get; set; }

        /// <summary>
        ///     The client identifier of a batch message.
        ///     If set, the identifier will be added in the delivery report/callback of this batch
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     f set to true, then
        ///     <see
        ///         href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Batches/#tag/Batches/operation/deliveryFeedback">
        ///         feedback
        ///     </see>
        ///     is expected after successful delivery.
        /// </summary>
        public bool FeedbackEnabled { get; set; } = false;

        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        public bool FlashMessage { get; set; } = false;

        /// <summary>
        ///     If set to true the message will be shortened when exceeding one part.
        /// </summary>
        public bool TruncateConcat { get; set; }

        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts
        /// </summary>
        public int? MaxNumberOfMessageParts { get; set; }

        /// <summary>
        ///     The type of number for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromTon { get; set; }

        /// <summary>
        ///     Number Plan Indicator for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromNpi { get; set; }
    }
}