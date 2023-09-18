using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    
    [JsonConverter(typeof(EnumRecordJsonConverter<ProcessingStrategy>))]
    public record ProcessingStrategy(string Value) : EnumRecord(Value)
    {
        public static readonly ProcessingStrategy Default = new("DEFAULT");
        public static readonly ProcessingStrategy DispatchOnly = new("DISPATCH_ONLY");
    }
}
