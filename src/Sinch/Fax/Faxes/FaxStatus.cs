using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// The status of the fax
    /// </summary>
    /// <value>The status of the fax</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<FaxStatus>))]
    public record FaxStatus(string Value) : EnumRecord(Value)
    {

        public static readonly FaxStatus Queued = new("QUEUED");
        public static readonly FaxStatus InProgress = new("IN_PROGRESS");
        public static readonly FaxStatus Completed = new("COMPLETED");
        public static readonly FaxStatus Failure = new("FAILURE");
    }

}
