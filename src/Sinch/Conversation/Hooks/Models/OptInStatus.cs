using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     Status of the opt-in registration.
    /// </summary>
    /// <value>Status of the opt-in registration.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<OptInStatus>))]
    public record OptInStatus(string Value) : EnumRecord(Value)
    {
        public static readonly OptInStatus OptInSucceeded = new("OPT_IN_SUCCEEDED");
        public static readonly OptInStatus OptInFailed = new("OPT_IN_FAILED");
        public static readonly OptInStatus OptInStatusUnspecified = new("OPT_IN_STATUS_UNSPECIFIED");
    }
}
