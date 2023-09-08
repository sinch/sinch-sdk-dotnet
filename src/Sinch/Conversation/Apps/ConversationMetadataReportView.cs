using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    [JsonConverter(typeof(SinchEnumConverter<ConversationMetadataReportView>))]
    public enum ConversationMetadataReportView
    {
        [EnumMember(Value = "NONE")]
        None,

        [EnumMember(Value = "FULL")]
        Full
    }
}
