using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     Represents the SMS type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<SmsType>))]
    public record SmsType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a text SMS type.
        /// </summary>
        public static readonly SmsType Text = new("mo_text");

        /// <summary>
        ///     Represents a binary SMS type.
        /// </summary>
        public static readonly SmsType Binary = new("mo_binary");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
