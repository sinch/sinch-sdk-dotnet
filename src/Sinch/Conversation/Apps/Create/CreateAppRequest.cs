using System.Collections.Generic;
using System.Text;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Apps.Create
{
    /// <summary>
    ///     The request sent to the API endpoint to create a new app.
    /// </summary>
    public sealed class CreateAppRequest
    {
        /// <summary>
        /// Gets or Sets ConversationMetadataReportView
        /// </summary>
        public ConversationMetadataReportView ConversationMetadataReportView { get; set; }

        /// <summary>
        ///     An array of channel credentials. The order of the credentials defines the app channel priority.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<ConversationChannelCredential> ChannelCredentials { get; set; }
#else
        public List<ConversationChannelCredential> ChannelCredentials { get; set; }
#endif


        /// <summary>
        ///     The display name for the app.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string DisplayName { get; set; }
#else
        public string DisplayName { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets RetentionPolicy
        /// </summary>
        public RetentionPolicy RetentionPolicy { get; set; }


        /// <summary>
        ///     Gets or Sets DispatchRetentionPolicy
        /// </summary>
        public DispatchRetentionPolicy DispatchRetentionPolicy { get; set; }


        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app. For more information, see [Processing Modes](../../../../../conversation/processing-modes/).
        /// </summary>
        public ProcessingMode ProcessingMode { get; set; }


        /// <summary>
        ///     Gets or Sets SmartConversation
        /// </summary>
        public SmartConversation SmartConversation { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppCreateRequest {\n");
            sb.Append("  ChannelCredentials: ").Append(ChannelCredentials).Append("\n");
            sb.Append("  ConversationMetadataReportView: ").Append(ConversationMetadataReportView).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  RetentionPolicy: ").Append(RetentionPolicy).Append("\n");
            sb.Append("  DispatchRetentionPolicy: ").Append(DispatchRetentionPolicy).Append("\n");
            sb.Append("  ProcessingMode: ").Append(ProcessingMode).Append("\n");
            sb.Append("  SmartConversation: ").Append(SmartConversation).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
