using System.Text.Json.Nodes;

namespace Sinch.Conversation.Events.EventTypes
{
    /// <summary>
    ///     Event that contains only a flexible payload field.
    /// </summary>
    public sealed class GenericEvent
    {
        /// <summary>
        ///     Arbitrary data set to the event. A valid JSON object.
        /// </summary>
        public JsonObject? Payload { get; set; }
    }
}
