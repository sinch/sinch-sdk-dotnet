using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Applications.QueryNumber
{
    /// <summary>
    ///     The type of the number.
    /// </summary>
    /// <value>The type of the number.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<NumberType>))]
    public record NumberType(string Value) : EnumRecord(Value)
    {
        public static readonly NumberType Unknown = new("Unknown");
        public static readonly NumberType Fixed = new("Fixed");
        public static readonly NumberType Mobile = new("Mobile");
        public static readonly NumberType Other = new("Other");
    }
}
