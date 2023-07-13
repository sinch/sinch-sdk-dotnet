using System;
using System.Collections.Generic;

namespace Sinch.SMS.Groups
{
    public class Group
    {
        /// <summary>
        ///     The ID used to reference this group.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Name of group, if set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The number of members currently in the group.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     Timestamp for group creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Timestamp for when the group was last updated.
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        ///     Phone numbers (MSISDNs) of child group will be included in this group.
        ///     If present, this group will be auto populated. Constraints: Elements must be group IDs.
        /// </summary>
        public HashSet<string> ChildGroups { get; set; }

        public AutoUpdate AutoUpdate { get; set; }
    }

    public class AutoUpdate
    {
        /// <summary>
        ///     Short code or long number addressed in
        ///     <see href="https://community.sinch.com/t5/Glossary/MO-Mobile-Originated/ta-p/7618">MO</see>.
        ///     Constraints: Must be valid MSISDN or short code.
        /// </summary>
        public string To { get; set; }

        public string Add { get; set; }

        public string Remove { get; set; }
    }
}
