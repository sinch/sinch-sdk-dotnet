using System.Collections.Generic;

namespace Sinch.Conversation.Contacts.List
{
    public sealed class ListIdentityConflictsResponse
    {
        /// <summary>
        ///     Token that should be included in the next list identity conflicts request to fetch the next page.
        /// </summary>
        public string? NextPageToken { get; set; }

        /// <summary>
        ///     List of identity conflicts belonging to the specified project.
        /// </summary>
        public List<IdentityConflict>? Conflicts { get; set; }
    }
}
