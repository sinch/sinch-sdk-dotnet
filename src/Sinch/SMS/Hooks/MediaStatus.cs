using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// Media item upload status
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MediaStatus
    {
        /// <summary>
        /// Media was successfully uploaded
        /// </summary>
        Uploaded,

        /// <summary>
        /// Media upload failed (check Code for reason)
        /// </summary>
        Failed
    }
}
