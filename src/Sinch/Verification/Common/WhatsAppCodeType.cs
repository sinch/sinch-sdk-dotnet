using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Common
{

    [JsonConverter(typeof(EnumRecordJsonConverter<WhatsAppCodeType>))]
    public record WhatsAppCodeType(string Value) : EnumRecord(Value)
    {
        public static readonly WhatsAppCodeType Numeric = new("Numeric");
        public static readonly WhatsAppCodeType Alpha = new("Alpha");
        public static readonly WhatsAppCodeType Alphanumeric = new("Alphanumeric");
    }

}
