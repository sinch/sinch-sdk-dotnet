namespace Sinch.Conversation.Contacts.List
{
    public sealed class ListIdentityConflictsRequest
    {
        /// <summary>
        ///     Optional. The maximum number of conflicts to fetch. The default is 10 and the maximum is 20.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Optional. Next page token previously returned if any.
        /// </summary>
        public string? PageToken { get; set; }
    }
}
