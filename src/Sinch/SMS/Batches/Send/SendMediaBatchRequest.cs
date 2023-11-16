using System;
using System.Collections.Generic;

namespace Sinch.SMS.Batches.Send
{
    /// <summary>
    ///    Only available in the US. Contact support if you wish to send MMS.
    /// </summary>
    public class SendMediaBatchRequest : BatchBase, ISendBatchRequest
    {
        /// <summary>
        ///     The message content, including a URL to the media file
        /// </summary>
#if NET7_0_OR_GREATER
        public required MediaBody Body { get; set; }
#else
        public MediaBody Body { get; set; }
#endif
        /// <summary>
        ///     MMS
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtMedia;

        /// <summary>
        ///     Default: <c>false</c>.<br/><br/>
        ///     Whether or not you want the media included in your message to be checked against
        ///     <see href="https://developers.sinch.com/docs/mms/bestpractices/">Sinch MMS channel best practices</see>.
        ///     If set to true, your message will be rejected if it doesn't conform to the listed
        ///     recommendations, otherwise no validation will be performed.
        /// </summary>
        public bool? StrictValidation { get; set; } 
        
        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parameters { get; set; }
    }
    
    public class MediaBody
    {
        /// <summary>
        ///     URL to the media file
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///   The message text. Text only media messages will be rejected, please use SMS instead.  
        /// </summary>
        public string Message { get; set; }
    }
}
