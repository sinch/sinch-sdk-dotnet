using Sinch.Core;

namespace Sinch.Voice.Callout
{
    /// <summary>
    ///     Determines what DTMF mode the participant will use in the call.
    /// </summary>
    /// <param name="Value"></param>
    public record DtmfMode(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Nothing is done with the participant's DTMF signals. This is the default mode. Any DTMF signals that the
        ///     participant sends can still be heard by all participants, but no action will be performed.
        /// </summary>
        public static readonly DtmfMode Ignore = new("ignore");

        /// <summary>
        ///     The participant's DTMF signals are forwarded to all other participants in the conference.
        /// </summary>
        public static readonly DtmfMode Forward = new("forward");

        /// <summary>
        ///     The participant's DTMF signals are detected by the conference and sent to your backend server using a
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/pie">
        ///         Prompt
        ///         Input Event
        ///     </see>
        ///     (PIE) callback.
        /// </summary>
        public static readonly DtmfMode Detect = new("detect");
    }
}
