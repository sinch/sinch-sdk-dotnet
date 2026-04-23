using System.Text.Json.Serialization;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     Marker interface for all sinch events
    /// </summary>
    [JsonConverter(typeof(ConversationSinchEventConverter))]
    public interface IConversationSinchEvent
    {
    }
}
