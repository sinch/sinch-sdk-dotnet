using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Batches.DryRun
{
    public class Request
    {
        /// <summary>
        ///     Whether to include per recipient details in the response
        /// </summary>
        [JsonIgnore]
        public bool PerRecipient { get; set; }

        /// <summary>
        ///     Max number of recipients to include per recipient details for in the response
        /// </summary>
        [JsonIgnore]
        public int NumberOfRecipients { get; set; } = 100;

        /// <summary>
        ///     List of Phone numbers and group IDs that will receive the batch. Constraints: 1 to 1000 elements
        ///     <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">More info</see>
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<string> To { get; set; }
#else
        public List<string> To { get; set; }
#endif


        /// <summary>
        ///     The message content. Normal text string for mt_text and Base64 encoded for mt_binary.<br /><br />
        ///     Max 1600 chars for mt_text and max 140 bytes together with udh for mt_binary.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Body { get; set; }
#else
        public string Body { get; set; }
#endif


        /// <summary>
        ///     Sender ID. This can be a phone number or an alphanumeric sender.<br /><br />
        ///     Required if Automatic Default Originator not configured.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///     Identifies the type of batch message.
        /// </summary>
        public SmsType Type { get; set; }

        /// <summary>
        ///     The UDH header of a binary message. Max 140 bytes together with body.<br /><br />Required if type is mt_binary.
        /// </summary>
        public string Udh { get; set; }

        /// <summary>
        ///     Request delivery report callback. <br /><br />
        ///     Note that delivery reports can be fetched from the API regardless of this setting.
        /// </summary>
        public DeliveryReport DeliveryReport { get; set; }

        /// <summary>
        ///     If set in the future, the message will be delayed until send_at occurs. <br /><br />
        ///     Must be before expire_at.<br /><br />
        ///     If set in the past, messages will be sent immediately.
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        ///     If set, the system will stop trying to deliver the message at this point. <br /><br />
        ///     Must be after send_at. Default is 3 days after send_at.
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        ///     Override the default callback URL for this batch. <br /><br />
        ///     Must be valid URL. <br /><br />
        ///     Max 2048 characters long.
        /// </summary>
        public Uri CallbackUrl { get; set; }

        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        public bool? FlashMessage { get; set; }

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient. <br /><br />
        ///     Not applicable to if type is mt_binary. <br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">Learn More.</see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parameters { get; set; }

        /// <summary>
        ///     The client identifier of a batch message.
        ///     If set, the identifier will be added in the delivery report/callback of this batch <br /><br />
        ///     Max 128 characters long
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts <br /><br />
        ///     Must be higher or equal 1
        /// </summary>
        public int? MaxNumberOfMessageParts { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("per_recipient", PerRecipient.ToString().ToLowerInvariant()));
            kvp.Add(new KeyValuePair<string, string>("number_of_recipients", NumberOfRecipients.ToString()));
            return StringUtils.ToQueryString(kvp);
        }
    }
}
