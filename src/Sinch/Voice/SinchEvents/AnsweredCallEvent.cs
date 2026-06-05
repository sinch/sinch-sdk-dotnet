using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     This Sinch event is sent when the call is picked up by the callee (person receiving the call). It's a POST request to
    ///     the specified event destination URL. Look here for allowed
    ///     [instructions](https://developers.sinch.com/docs/voice/api-reference/svaml/instructions) and
    ///     [actions](https://developers.sinch.com/docs/voice/api-reference/svaml/actions).
    ///     If there is no response to the ACE event within the timeout period, the call is connected.
    ///     If you have [Answering Machine Detection (AMD)](https://developers.sinch.com/docs/voice/api-reference/amd_v2)
    ///     enabled, the amd object will also be present on ACE events.
    ///     Note: ACE events are not issued for InApp Calls (destination: username), only PSTN and SIP calls.
    /// </summary>
    public sealed class AnsweredCallEvent : VoiceCallSinchEvent
    {
        /// <summary>
        ///     Must have the value ace.
        /// </summary>
        [JsonPropertyName("event")]
        [JsonInclude]
        internal override EventType Event { get; set; } = EventType.AnsweredCallEvent;


        /// <summary>
        ///     If [Answering Machine Detection (AMD)](https://developers.sinch.com/docs/voice/api-reference/amd_v2) is enabled,
        ///     this object contains information about whether the call was answered by a machine.
        /// </summary>
        [JsonPropertyName("amd")]
        public AnsweringMachineDetection? Amd { get; set; }

    }
}
