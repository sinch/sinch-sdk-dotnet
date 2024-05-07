using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Apps.Update
{
    /// <summary>
    ///     Updates a particular app as specified by the App ID. Note that this is a PATCH operation,
    ///     so any specified field values will replace existing values.
    ///     Therefore, if you'd like to add additional configurations to an existing Conversation API app,
    ///     ensure that you include existing values AND new values in the call. For example, if you'd like
    ///     to add new channel_credentials, you can get your existing Conversation API app, extract the existing
    ///     channel_credentials list, append your new configuration to that list, and include the updated channel_credentials
    ///     list in this update call.
    /// </summary>
    public sealed class UpdateAppRequest
    {
        /// <summary>
        ///     The set of field mask paths.
        /// </summary>
        [JsonIgnore]
        public List<string>? UpdateMaskPaths { get; set; }

        /// <summary>
        /// Gets or Sets ConversationMetadataReportView
        /// </summary>
        public ConversationMetadataReportView? ConversationMetadataReportView { get; set; }

        /// <summary>
        ///     An array of channel credentials. The order of the credentials defines the app channel priority.
        /// </summary>
        public List<ConversationChannelCredential>? ChannelCredentials { get; set; }


        /// <summary>
        ///     The display name for the app.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string DisplayName { get; set; }
#else
        public string DisplayName { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets RetentionPolicy
        /// </summary>
        public RetentionPolicy? RetentionPolicy { get; set; }


        /// <summary>
        ///     Gets or Sets DispatchRetentionPolicy
        /// </summary>
        public DispatchRetentionPolicy? DispatchRetentionPolicy { get; set; }


        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app. For more information, see [Processing Modes](../../../../../conversation/processing-modes/).
        /// </summary>
        public ProcessingMode? ProcessingMode { get; set; }


        /// <summary>
        ///     Gets or Sets SmartConversation
        /// </summary>
        public SmartConversation? SmartConversation { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppCreateRequest {\n");
            sb.Append("  UpdateMaskPaths: ").Append(UpdateMaskPaths).Append("\n");
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
