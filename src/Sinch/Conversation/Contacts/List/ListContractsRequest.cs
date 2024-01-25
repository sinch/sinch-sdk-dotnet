namespace Sinch.Conversation.Contacts.List
{
    public class ListContactsRequest
    {
        /// <summary>
        ///     Optional. The maximum number of contacts to fetch. The default is 10 and the maximum is 20.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Optional. Next page token previously returned if any.
        /// </summary>
        public string PageToken { get; set; }

        /// <summary>
        ///     Optional. Contact identifier in an external system. If used, channel and identity query parameters can't be used.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        ///     Optional. Specifies a channel, and must be set to one of the enum values. If set, the identity parameter must be
        ///     set and external_id can't be used. Used in conjunction with identity to uniquely identify the specified channel
        ///     identity.
        /// </summary>
        public ConversationChannel Channel { get; set; }

        /// <summary>
        ///     Optional. If set, the channel parameter must be set and external_id can't be used. Used in conjunction with channel
        ///     to uniquely identify the specified channel identity. This will differ from channel to channel. For example, a phone
        ///     number for SMS, WhatsApp, and Viber Business.
        /// </summary>
        public string Identity { get; set; }
    }
}
