using System;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     A Voice Sinch Event that carries the common call fields defined for calls.
    ///     Implemented by <see cref="IncomingCallEvent" />, <see cref="AnsweredCallEvent" />,
    ///     and <see cref="DisconnectedCallEvent" />.
    /// </summary>
    public interface IVoiceCallSinchEvent : IVoiceSinchEvent
    {
        /// <summary>
        ///     The timestamp in UTC format.
        /// </summary>
        DateTime? Timestamp { get; }

        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        string? Custom { get; }

        /// <summary>
        ///     The unique application key. You can find it in the Sinch
        ///     <a href="https://dashboard.sinch.com/voice/apps">dashboard</a>.
        /// </summary>
        string? ApplicationKey { get; }
    }
}
