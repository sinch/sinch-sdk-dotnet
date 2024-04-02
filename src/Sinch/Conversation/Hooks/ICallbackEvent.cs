using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     Marker interface for all callback events
    /// </summary>
    [JsonInterfaceConverter(typeof(InterfaceConverter<ICallbackEvent>))]
    public interface ICallbackEvent
    {
    }
}
