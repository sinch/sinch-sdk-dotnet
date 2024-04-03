using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Webhooks
{
    /// <summary>
    ///     Defines WebhookTargetType
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<WebhookTargetType>))]
    public record WebhookTargetType(string Value) : EnumRecord(Value)
    {
        public static readonly WebhookTargetType Dismiss = new("DISMISS");
        public static readonly WebhookTargetType Http = new("HTTP");
    }
}
