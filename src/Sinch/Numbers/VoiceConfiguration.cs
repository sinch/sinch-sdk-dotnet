using System;
using System.Text.Json.Serialization;

namespace Sinch.Numbers
{
    public sealed class VoiceConfiguration
    {
        /// <summary>
        ///     Your app ID for the Voice API.
        ///     The appId can be found in your <see href="https://dashboard.sinch.com/voice/apps">Sinch Customer Dashboard</see>
        ///     under Voice, then apps.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; } = null!;
#endif


        /// <summary>
        ///     This object is temporary and will appear while the scheduled voice provisioning is processing.
        ///     Once it has successfully processed, only the ID of the Voice configuration will display.
        /// </summary>
        [JsonInclude]
        public ScheduledVoiceProvisioning? ScheduledVoiceProvisioning { get; private set; }

        /// <summary>
        ///     Timestamp when the status was last updated.
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }
    }
}
