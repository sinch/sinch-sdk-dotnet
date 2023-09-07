using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     Marker interface to deserialize polymorphic sms 
    /// </summary>
    [JsonConverter(typeof(InterfaceConverter<IIncomingSms>))]
    public interface IIncomingSms
    {
    }
}
