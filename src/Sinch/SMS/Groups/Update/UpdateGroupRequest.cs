using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Groups.Update
{
    public sealed class UpdateGroupRequest : IGroupUpdateRequest
    {
        /// <summary>
        ///     ID of a group that you are interested in getting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string GroupId { get; set; }
#else
        public string GroupId { get; set; } = null!;
#endif

        /// <summary>
        ///     The name of the group.
        ///     Set to null to remove name.
        ///     Set to string.Empty to leave the name unchanged.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        ///     Add a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Add { get; set; }

        /// <summary>
        ///     Remove a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Remove { get; set; }

        /// <summary>
        ///     Copy the members from the another group into this group.
        ///     <br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? AddFromGroup { get; set; }

        /// <summary>
        ///     Remove the members in a specified group from this group.<br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? RemoveFromGroup { get; set; }

        public AutoUpdateEdit? AutoUpdate { get; set; }
    }

    // for ignoring required GroupId
    internal interface IGroupUpdateRequest
    {
        /// <summary>
        ///     The name of the group.
        ///     Set to null to remove name.
        ///     Set to string.Empty to leave the name unchanged.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? Name { get; set; }

        /// <summary>
        ///     Add a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Add { get; set; }

        /// <summary>
        ///     Remove a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Remove { get; set; }

        /// <summary>
        ///     Copy the members from the another group into this group.
        ///     <br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? AddFromGroup { get; set; }

        /// <summary>
        ///     Remove the members in a specified group from this group.<br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? RemoveFromGroup { get; set; }

        public AutoUpdateEdit? AutoUpdate { get; set; }
    }

    /// <summary>
    ///     Workaround to omit name property
    /// </summary>
    internal class RequestWithoutName
    {
        /// <summary>
        ///     Add a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Add { get; set; }

        /// <summary>
        ///     Remove a list of phone numbers (MSISDNs) to this group.
        ///     The phone numbers are a strings within an array and must be in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format.
        /// </summary>
        public List<string>? Remove { get; set; }

        /// <summary>
        ///     Copy the members from the another group into this group.
        ///     <br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? AddFromGroup { get; set; }

        /// <summary>
        ///     Remove the members in a specified group from this group.<br /><br />
        ///     Constraints: Must be valid group ID
        /// </summary>
        public string? RemoveFromGroup { get; set; }

        public AutoUpdateEdit? AutoUpdate { get; set; }
    }
}
