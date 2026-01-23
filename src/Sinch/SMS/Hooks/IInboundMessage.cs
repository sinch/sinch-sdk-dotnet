using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     Base interface for all inbound message types.
    /// </summary>
    /// <seealso cref="Sinch.SMS.Webhooks.ISmsWebhooks.ParseEvent"/>
    [JsonConverter(typeof(InterfaceConverter<IInboundMessage>))]
    public interface IInboundMessage : ISmsEvent
    {
    }
}
