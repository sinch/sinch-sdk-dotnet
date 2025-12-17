using Sinch.Core;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     Marker interface for all callback events
    /// </summary>
    [JsonConverter(typeof(CallbackEventConverter))]
    //[JsonInterfaceConverter(typeof(InterfaceConverter<ICallbackEvent>))]
    public interface ICallbackEvent
    {
    }
}
