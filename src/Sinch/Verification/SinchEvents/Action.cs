using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.SinchEvents
{
    [JsonConverter(typeof(EnumRecordJsonConverter<Action>))]
    public record Action(string Value) : EnumRecord(Value)
    {
        public static readonly Action Allow = new("allow");
        public static readonly Action Deny = new("deny");
    }
}
