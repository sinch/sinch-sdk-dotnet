using System;
using System.Text.Json.Serialization;
using Sinch.Voice.Calls.Actions;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     This callback is made when the call is picked up by the callee (person receiving the call). It's a POST request to
    ///     the specified calling callback URL. Look here for allowed
    ///     [instructions](https://developers.sinch.com/docs/voice/api-reference/svaml/instructions) and
    ///     [actions](https://developers.sinch.com/docs/voice/api-reference/svaml/actions).
    ///     If there is no response to the callback within the timeout period, the call is connected.
    ///     If you have [Answering Machine Detection (AMD)](https://developers.sinch.com/docs/voice/api-reference/amd_v2)
    ///     enabled, the amd object will also be present on ACE callbacks.
    ///     Note: ACE Callbacks are not issued for InApp Calls (destination: username), only PSTN and SIP calls.
    /// </summary>
    public sealed class AnsweredCallEvent : IVoiceEvent
    {
        /// <summary>
        ///     Must have the value ace.
        /// </summary>
        [JsonPropertyName("event")]
        [JsonInclude]
        internal override EventType Event { get; set; } = EventType.AnsweredCallEvent;


        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callid")]
        public string? CallId { get; set; }


        /// <summary>
        ///     The path of the API resource.
        /// </summary>
        [JsonPropertyName("callResourceUrl")]
        public string? CallResourceUrl { get; set; }


        /// <summary>
        ///     The timestamp in UTC format.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }


        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }


        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }

        /// <summary>
        ///     If [Answering Machine Detection (AMD)](https://developers.sinch.com/docs/voice/api-reference/amd_v2) is enabled,
        ///     this object contains information about whether the call was answered by a machine.
        /// </summary>
        [JsonPropertyName("amd")]
        public AnsweringMachineDetection? Amd { get; set; }
        
        /// <summary>
        ///     The unique application key. You can find it in the Sinch [dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("applicationKey")]
        public string? ApplicationKey { get; set; }
    }
}
