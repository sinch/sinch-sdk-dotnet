using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    ///     The content type for delivering Sinch events to the event destination URL.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<EventDestinationContentType>))]
    public record EventDestinationContentType(string Value) : EnumRecord(Value)
    {
        public static readonly EventDestinationContentType MultipartFormData = new("multipart/form-data");
        public static readonly EventDestinationContentType ApplicationJson = new("application/json");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
