using System.Collections.Generic;

namespace Sinch.SMS.Groups.List
{
    public class ListGroupsResponse
    {
        /// <summary>
        ///     The requested page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///     The number of groups returned in this request
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     The total number of groups.
        /// </summary>
        public int Count { get; set; }

        public List<Group> Groups { get; set; } = new();
    }
}
