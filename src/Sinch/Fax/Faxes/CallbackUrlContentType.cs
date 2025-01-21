using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// The content type of the callback.
    /// </summary>
    /// <value>The content type of the callback.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallbackUrlContentType>))]
    public record CallbackUrlContentType(string Value) : EnumRecord(Value)
    {
        public static readonly CallbackUrlContentType MultipartFormData = new("multipart/form-data");
        public static readonly CallbackUrlContentType ApplicationJson = new("application/json");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
