using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// Media item upload status
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MediaStatus>))]
    public record MediaStatus(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// Media was successfully uploaded
        /// </summary>
        public static readonly MediaStatus Uploaded = new("Uploaded");
        /// <summary>
        /// Media upload failed
        /// </summary>
        public static readonly MediaStatus Failed = new("Failed");
    }
}
