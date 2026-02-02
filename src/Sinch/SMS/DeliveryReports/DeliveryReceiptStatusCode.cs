namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Delivery receipt status codes.
    ///     The REST API error codes are a combination of SMPP error codes, MMS error codes and custom codes.
    ///     See <see href="https://developers.sinch.com/docs/sms/api-reference/sms/delivery-reports/delivery-report-error-codes">Delivery Report Error Codes</see>.
    /// </summary>
    public enum DeliveryReceiptStatusCode
    {
        /// <summary>
        ///     Queued. Message is queued within REST API system and will be dispatched according to the rate of the account.
        /// </summary>
        Queued = 400,

        /// <summary>
        ///     Dispatched. Message has been dispatched to SMSC.
        /// </summary>
        Dispatched = 401,

        /// <summary>
        ///     Message unroutable. SMSC rejected message. Retrying is likely to cause the same error.
        /// </summary>
        MessageUnroutable = 402,

        /// <summary>
        ///     Internal error. An unexpected error caused the message to fail.
        /// </summary>
        InternalError = 403,

        /// <summary>
        ///     Temporary delivery failure. Message failed because of temporary delivery failure. Message can be retried.
        /// </summary>
        TemporaryDeliveryFailure = 404,

        /// <summary>
        ///     Unmatched Parameter. One or more parameters in the message body has no mapping for this recipient.
        ///     See <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">Message Parameterization</see>.
        /// </summary>
        UnmatchedParameter = 405,

        /// <summary>
        ///     Internal Expiry. Message was expired before reaching SMSC. This may happen if the expiry time for the message was very short.
        /// </summary>
        InternalExpiry = 406,

        /// <summary>
        ///     Cancelled. Message was cancelled by user before reaching SMSC.
        /// </summary>
        Cancelled = 407,

        /// <summary>
        ///     Internal Reject. SMSC rejected the message. Retrying is likely to cause the same error.
        /// </summary>
        InternalReject = 408,

        /// <summary>
        ///     Unmatched default originator. No default originator exists/configured for this recipient when sending message without originator.
        /// </summary>
        UnmatchedDefaultOriginator = 410,

        /// <summary>
        ///     Exceeded parts limit. Message failed as the number of message parts exceeds the defined max number of message parts.
        /// </summary>
        ExceededPartsLimit = 411,

        /// <summary>
        ///     Unprovisioned region. SMSC rejected the message. The account hasn't been provisioned for this region.
        /// </summary>
        UnprovisionedRegion = 412,

        /// <summary>
        ///     Blocked. The account is blocked. Reach out to support for help. Potentially out of credits.
        /// </summary>
        Blocked = 413,

        /// <summary>
        ///     Bad Media. MMS only, the request failed due to a bad media URL. It is possible that the URL was unreachable, or sent a bad response.
        /// </summary>
        BadMedia = 414,

        /// <summary>
        ///     Delivery report Rejected. MMS only, message reached MMSC but was rejected by MMS gateway or mobile network.
        /// </summary>
        DeliveryReportRejected = 415,

        /// <summary>
        ///     Delivery report Not Supported. MMS only, message reached MMSC but it is not supported.
        /// </summary>
        DeliveryReportNotSupported = 416,

        /// <summary>
        ///     Delivery report Unreachable. MMS only, message reached MMSC but the destination network or the mobile subscriber cannot be reached.
        /// </summary>
        DeliveryReportUnreachable = 417,

        /// <summary>
        ///     Delivery report Unrecognized. MMS only, message reached MMSC but the handset of the mobile subscriber does not recognize the message content.
        /// </summary>
        DeliveryReportUnrecognized = 418
    }
}
