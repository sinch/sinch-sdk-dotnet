using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Represents the conversation metadata report view options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ConversationMetadataReportView>))]
    public record ConversationMetadataReportView(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     No metadata report view.
        /// </summary>
        public static readonly ConversationMetadataReportView None = new("NONE");

        /// <summary>
        ///     Full metadata report view.
        /// </summary>
        public static readonly ConversationMetadataReportView Full = new("FULL");
    }
}
