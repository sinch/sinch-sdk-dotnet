using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Apps.Create;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Apps
{
    public sealed class App
    {
        /// <summary>
        ///     An array of channel credentials. The order of the credentials defines the app channel priority.
        /// </summary>
        public List<ConversationChannelCredentials>? ChannelCredentials { get; set; }

        /// <summary>
        ///     Gets or Sets ConversationMetadataReportView
        /// </summary>
        public ConversationMetadataReportView? ConversationMetadataReportView { get; set; }

        /// <summary>
        ///     The display name for the app.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        ///     The ID of the app. You can find this on the [Sinch Dashboard](https://dashboard.sinch.com/convapi/apps).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Gets or Sets RateLimits
        /// </summary>
        public RateLimits? RateLimits { get; set; }


        /// <summary>
        ///     Gets or Sets RetentionPolicy
        /// </summary>
        public RetentionPolicy? RetentionPolicy { get; set; }

        /// <summary>
        ///     Gets or Sets DispatchRetentionPolicy
        /// </summary>
        public DispatchRetentionPolicy? DispatchRetentionPolicy { get; set; }

        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app.
        ///     For more information,
        ///     see [Processing Modes](https://developers.sinch.com/docs/conversation/processing-modes/).
        /// </summary>
        public ProcessingMode? ProcessingMode { get; set; }


        /// <summary>
        ///     Gets or Sets SmartConversation
        /// </summary>
        public SmartConversation? SmartConversation { get; set; }


        /// <summary>
        ///     Gets or Sets QueueStats
        /// </summary>
        public QueueStats? QueueStats { get; set; }

        /// <summary>
        ///     Gets or Sets CallbackSettings
        /// </summary>
        [JsonPropertyName("callback_settings")]
        public CallbackSettings? CallbackSettings { get; set; }

        /// <summary>
        ///     Gets or Sets DeliveryReportBasedFallback
        /// </summary>
        [JsonPropertyName("delivery_report_based_fallback")]
        public DeliveryReportBasedFallback? DeliveryReportBasedFallback { get; set; }


        /// <summary>
        ///     Gets or Sets MessageRetrySettings
        /// </summary>
        [JsonPropertyName("message_retry_settings")]
        public MessageRetrySettings? MessageRetrySettings { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(App)} {{\n");
            sb.Append($"  {nameof(ChannelCredentials)}: ").Append(ChannelCredentials).Append('\n');
            sb.Append($"  {nameof(ConversationMetadataReportView)}: ").Append(ConversationMetadataReportView).Append('\n');
            sb.Append($"  {nameof(DisplayName)}: ").Append(DisplayName).Append('\n');
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(RateLimits)}: ").Append(RateLimits).Append('\n');
            sb.Append($"  {nameof(RetentionPolicy)}: ").Append(RetentionPolicy).Append('\n');
            sb.Append($"  {nameof(DispatchRetentionPolicy)}: ").Append(DispatchRetentionPolicy).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append($"  {nameof(SmartConversation)}: ").Append(SmartConversation).Append('\n');
            sb.Append($"  {nameof(QueueStats)}: ").Append(QueueStats).Append('\n');
            sb.Append($"  {nameof(CallbackSettings)}: ").Append(CallbackSettings).Append('\n');
            sb.Append($"  {nameof(DeliveryReportBasedFallback)}: ").Append(DeliveryReportBasedFallback).Append('\n');
            sb.Append($"  {nameof(MessageRetrySettings)}: ").Append(MessageRetrySettings).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
