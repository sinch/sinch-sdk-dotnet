using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Report.Response
{
    /// <summary>
    ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
    /// </summary>
    /// <param name="Value"></param>
    [JsonConverter(typeof(EnumRecordJsonConverter<Source>))]
    public record Source(string Value) : EnumRecord(Value)
    {
        public static readonly Source Intercepted = new("intercepted");
        public static readonly Source Manual = new("manual");
    }
}
