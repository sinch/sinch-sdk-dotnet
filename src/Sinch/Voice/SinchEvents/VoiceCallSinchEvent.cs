using System;
using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Base type for Voice Sinch Events such as:
    ///     <see cref="IncomingCallEvent" />, <see cref="AnsweredCallEvent" />, and
    ///     <see cref="DisconnectedCallEvent" />.
    /// </summary>
    public abstract class VoiceCallSinchEvent : VoiceSinchEvent, IVoiceCallSinchEvent
    {
        /// <inheritdoc />
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("applicationKey")]
        public string? ApplicationKey { get; set; }
    }
}
