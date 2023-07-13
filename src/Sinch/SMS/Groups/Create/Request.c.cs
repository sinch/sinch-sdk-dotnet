using System.Collections.Generic;

namespace Sinch.SMS.Groups.Create
{
    public sealed class Request
    {
        /// <summary>
        ///     Name of the group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Initial list of phone numbers in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see>
        ///     format <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">MSISDNs</see> for the group.
        /// </summary>
        public IEnumerable<string> Members { get; set; }

        /// <summary>
        ///     Phone numbers (<see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">MSISDNs</see>)
        ///     of child group will be included in this group.
        ///     If present then this group will be auto populated. <br /><br />
        ///     Elements must be group IDs.
        /// </summary>
        public IEnumerable<string> ChildGroups { get; set; }

        public AutoUpdateEdit AutoUpdate { get; set; }
    }
}
