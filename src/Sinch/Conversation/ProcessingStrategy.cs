using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    [JsonConverter(typeof(SinchEnumConverter<ProcessingStrategy>))]
    public enum ProcessingStrategy
    {
        [EnumMember(Value = "DEFAULT")]
        Default,

        [EnumMember(Value = "DISPATCH_ONLY")]
        DispatchOnly
    }
}
