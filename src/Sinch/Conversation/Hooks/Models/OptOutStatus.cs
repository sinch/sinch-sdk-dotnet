using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     Status of the opt-out registration.
    /// </summary>
    /// <value>Status of the opt-out registration.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<OptOutStatus>))]
    public record OptOutStatus(string Value) : EnumRecord(Value)
    {
        public static readonly OptOutStatus OptOutSucceeded = new("OPT_OUT_SUCCEEDED");
        public static readonly OptOutStatus OptOutFailed = new("OPT_OUT_FAILED");
        public static readonly OptOutStatus OptOutStatusUnspecified = new("OPT_OUT_STATUS_UNSPECIFIED");
    }
}
