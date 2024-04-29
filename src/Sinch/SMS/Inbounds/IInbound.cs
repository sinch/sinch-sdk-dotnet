using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    [JsonInterfaceConverter(typeof(InterfaceConverter<IInbound>))]
    public interface IInbound
    {
        public SmsType Type { get; }
    }
}
