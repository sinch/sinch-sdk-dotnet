using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.Hooks
{
    [JsonConverter(typeof(SinchEnumConverter<EventStatus>))]
    public enum EventStatus
    {
        [EnumMember(Value = "SUCCEEDED")]
        Succeeded,
        [EnumMember(Value = "FAILED")]
        Failed
    }
}
