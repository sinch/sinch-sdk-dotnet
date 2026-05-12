using Sinch.Core;
using Sinch.SMS.SinchEvents;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     Marker interface for Inbound types (SmsInbound, BinaryInbound, MediaInbound).
    ///     Supports deserialization from REST API responses via type discriminator.
    /// </summary>
    [JsonInterfaceConverter(typeof(InboundJsonConverter))]
    public interface IInbound : ISmsSinchEvent
    {
        InboundMessageType Type { get; }
    }
}
