using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    ///     The content type of the event destination payload.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<FaxContentType>))]
    public record FaxContentType(string Value) : EnumRecord(Value)
    {
        public static readonly FaxContentType MultipartFormData = new("multipart/form-data");
        public static readonly FaxContentType ApplicationJson = new("application/json");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
