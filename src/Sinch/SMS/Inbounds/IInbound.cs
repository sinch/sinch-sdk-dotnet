using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     Marker interface for Inbound types (SmsInbound, BinaryInbound, MediaInbound).
    ///     Supports deserialization from REST API responses via type discriminator.
    /// </summary>
    [JsonInterfaceConverter(typeof(InboundJsonConverter))]
    public interface IInbound : ISmsEvent
    {
        SmsType Type { get; init; }
    }
}
