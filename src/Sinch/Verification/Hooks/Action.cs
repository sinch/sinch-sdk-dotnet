using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Hooks
{
    [JsonConverter(typeof(EnumRecordJsonConverter<Action>))]
    public record Action(string Value) : EnumRecord(Value)
    {
        public static Action Allow = new("allow");
        public static Action Deny = new("deny");
    }
}
