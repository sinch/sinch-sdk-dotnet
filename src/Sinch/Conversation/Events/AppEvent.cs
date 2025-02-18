using System;
using System.Text.Json.Serialization;
using Sinch.Conversation.Events.AppEvents;
using Sinch.Conversation.Events.EventTypes;

namespace Sinch.Conversation.Events
{
    public sealed class AppEvent
    {
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public AppEvent()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="ComposingEvent" /> class
        /// </summary>
        /// <param name="composingEvent">An instance of ComposingEvent.</param>
        public AppEvent(ComposingEvent composingEvent)
        {
            ComposingEvent = composingEvent;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="ComposingEndEvent" /> class
        /// </summary>
        /// <param name="composingEndEvent">An instance of ComposingEndEvent.</param>
        public AppEvent(ComposingEndEvent composingEndEvent)
        {
            ComposingEndEvent = composingEndEvent;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="CommentReplyEvent" /> class
        /// </summary>
        /// <param name="commentReplyEvent">An instance of CommentReplyEvent.</param>
        public AppEvent(CommentReplyEvent commentReplyEvent)
        {
            CommentReplyEvent = commentReplyEvent;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="AgentJoinedEvent" /> class
        /// </summary>
        /// <param name="agentJoinedEvent">An instance of AgentJoinedEvent.</param>
        public AppEvent(AgentJoinedEvent agentJoinedEvent)
        {
            AgentJoinedEvent = agentJoinedEvent;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="AgentLeftEvent" /> class
        /// </summary>
        /// <param name="agentLeftEvent">An instance of AgentLeftEvent.</param>
        public AppEvent(AgentLeftEvent agentLeftEvent)
        {
            AgentLeftEvent = agentLeftEvent;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppEvent" /> class
        ///     with the <see cref="GenericEvent" /> class
        /// </summary>
        /// <param name="genericEvent">An instance of GenericEvent.</param>
        public AppEvent(GenericEvent genericEvent)
        {
            GenericEvent = genericEvent;
        }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEvent? ComposingEvent { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ComposingEndEvent? ComposingEndEvent { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CommentReplyEvent? CommentReplyEvent { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AgentJoinedEvent? AgentJoinedEvent { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AgentLeftEvent? AgentLeftEvent { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GenericEvent? GenericEvent { get; private set; }
    }
}
