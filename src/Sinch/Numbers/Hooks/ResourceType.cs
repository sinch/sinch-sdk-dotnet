using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(SinchEnumConverter<ResourceType>))]
    public enum ResourceType
    {
        [EnumMember(Value = "NUMBER")]
        Number,
        [EnumMember(Value = "HOSTING_ORDER")]
        HostingOrder,
        [EnumMember(Value = "Brand")]
        Brand
    }
}
