using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Sinch.SMS.Groups.Replace
{
    public sealed class ReplaceGroupRequest
    {
        /// <summary>
        ///     ID of a group that you are interested in getting.
        /// </summary>
        [JsonIgnore]
#if NET7_0_OR_GREATER
        public required string GroupId { get; init; }
#else
        public string GroupId { get; set; } = null!;
#endif

        /// <summary>
        ///     The initial members of the group.<br /><br />
        ///     Constraints: Elements must be phone numbers in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format MSISDNs.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<string> Members { get; init; }
#else
        public List<string> Members { get; set; } = null!;
#endif

        /// <summary>
        ///     Name of group
        /// </summary>
        public string? Name { get; set; }
    }

    // Workaround for not working  JsonIgnore with required keyword
    internal sealed class RequestInner
    {
        public List<string>? Members { get; set; }

        public string? Name { get; set; }
    }
}
