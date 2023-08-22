using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    /// <summary>
    ///     Select the priority type for the message.
    /// </summary>
    [JsonConverter(typeof(SinchEnumConverter<MessageQueue>))]
    public enum MessageQueue
    {
        [EnumMember(Value = "NORMAL_PRIORITY")]
        NormalPriority,

        [EnumMember(Value = "HIGH_PRIORITY")]
        HighPriority,
    }
}
