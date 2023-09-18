using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.Apps
{
    public class App
    {
        /// <summary>
        ///     An array of channel credentials. The order of the credentials defines the app channel priority.
        /// </summary>
        public List<ConversationChannelCredential> ChannelCredentials { get; set; }

        /// <summary>
        ///     Gets or Sets ConversationMetadataReportView
        /// </summary>
        public ConversationMetadataReportView ConversationMetadataReportView { get; set; }

        /// <summary>
        ///     The display name for the app.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     The ID of the app. You can find this on the [Sinch Dashboard](https://dashboard.sinch.com/convapi/apps).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or Sets RateLimits
        /// </summary>
        public RateLimits RateLimits { get; set; }


        /// <summary>
        ///     Gets or Sets RetentionPolicy
        /// </summary>
        public RetentionPolicy RetentionPolicy { get; set; }

        /// <summary>
        ///     Gets or Sets DispatchRetentionPolicy
        /// </summary>
        public DispatchRetentionPolicy DispatchRetentionPolicy { get; set; }

        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app.
        ///     For more information,
        ///     see [Processing Modes](https://developers.sinch.com/docs/conversation/processing-modes/).
        /// </summary>
        public ProcessingMode ProcessingMode { get; set; }


        /// <summary>
        ///     Gets or Sets SmartConversation
        /// </summary>
        public SmartConversation SmartConversation { get; set; }


        /// <summary>
        ///     Gets or Sets QueueStats
        /// </summary>
        public QueueStats QueueStats { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppResponse {\n");
            sb.Append("  ChannelCredentials: ").Append(ChannelCredentials).Append("\n");
            sb.Append("  ConversationMetadataReportView: ").Append(ConversationMetadataReportView).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RateLimits: ").Append(RateLimits).Append("\n");
            sb.Append("  RetentionPolicy: ").Append(RetentionPolicy).Append("\n");
            sb.Append("  DispatchRetentionPolicy: ").Append(DispatchRetentionPolicy).Append("\n");
            sb.Append("  ProcessingMode: ").Append(ProcessingMode).Append("\n");
            sb.Append("  SmartConversation: ").Append(SmartConversation).Append("\n");
            sb.Append("  QueueStats: ").Append(QueueStats).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
