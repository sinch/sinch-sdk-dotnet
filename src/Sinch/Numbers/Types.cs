using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    [JsonConverter(typeof(SinchEnumConverter<Types>))]
    public enum Types
    {
        /// <summary>
        ///     Numbers that belong to a specific range.
        /// </summary>
        [EnumMember(Value = "MOBILE")]
        Mobile,

        /// <summary>
        ///     Numbers that are assigned to a specific geographic region
        /// </summary>
        [EnumMember(Value = "LOCAL")]
        Local,

        /// <summary>
        ///     Number that are free of charge for the calling party but billed for all arriving calls.
        /// </summary>
        [EnumMember(Value = "TOLL_FREE")]
        TollFree
    }
}
