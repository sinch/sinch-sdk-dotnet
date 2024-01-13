using System.Collections.Generic;

namespace Sinch.Conversation.Contacts.List
{
    public class ListContactsResponse
    {
        /// <summary>
        ///     Token that should be included in the next list contacts request to fetch the next page.
        /// </summary>
        public string NextPageToken { get; set; }

        /// <summary>
        ///     List of contacts belonging to the specified project.
        /// </summary>
        public List<Contact> Contacts { get; set; }
    }
}
