﻿using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback delivers contact (end-user) messages to the API clients.
    /// </summary>
    public sealed class MessageInboundEvent
    {
        /// <summary>
        ///     Id of the subscribed app.
        /// </summary>
        [JsonPropertyName("app_id")]
        public string AppId { get; set; }


        /// <summary>
        ///     Timestamp marking when the channel callback was accepted/received by the Conversation API.
        /// </summary>
        [JsonPropertyName("accepted_time")]
        public DateTime AcceptedTime { get; set; }


        /// <summary>
        ///     Timestamp of the event as provided by the underlying channels.
        /// </summary>
        [JsonPropertyName("event_time")]
        public DateTime EventTime { get; set; }


        /// <summary>
        ///     The project ID of the app which has subscribed for the callback.
        /// </summary>
        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }


        /// <summary>
        ///     Context-dependent metadata. Refer to specific callback&#39;s documentation for exact information provided.
        /// </summary>
        [JsonPropertyName("message_metadata")]
        public string MessageMetadata { get; set; }


        /// <summary>
        ///     The value provided in field correlation_id of a send message request.
        /// </summary>
        [JsonPropertyName("correlation_id")]
        public string CorrelationId { get; set; }


        /// <summary>
        ///     Gets or Sets Message
        /// </summary>
        [JsonPropertyName("message")]
        public MessageInboundEventItem Message { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageInboundEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
