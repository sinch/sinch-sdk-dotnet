using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     A label, either SAFE or UNSAFE, that classifies the analyzed content.
    /// </summary>
    /// <value>A label, either SAFE or UNSAFE, that classifies the analyzed content.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<Evaluation>))]
    public record Evaluation(string Value) : EnumRecord(Value)
    {
        public static readonly Evaluation Safe = new("SAFE");
        public static readonly Evaluation Unsafe = new("UNSAFE");
    }
}
