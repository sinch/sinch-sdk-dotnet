using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.VoiceConfigurations
{
    /// <summary>
    /// The voice application type. Examples are RTC, EST, FAX
    /// </summary>
    /// <value>The voice application type. Examples are RTC, EST, FAX</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<VoiceApplicationType>))]
    public record VoiceApplicationType(string Value) : EnumRecord(Value)
    {

        public static readonly VoiceApplicationType Rtc = new("RTC");
        public static readonly VoiceApplicationType Est = new("EST");
        public static readonly VoiceApplicationType Fax = new("FAX");
    }
}
