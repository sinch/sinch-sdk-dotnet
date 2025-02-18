using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Calls.Instructions;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     The Incoming Call Event (ICE) or The Answered Call Event (ACE) requires a valid SVAML object in response.
    /// </summary>
    public sealed class CallEventResponse
    {
        /// <summary>
        ///     The collection of instructions that can perform various tasks during the call. You can include as many instructions
        ///     as necessary.
        /// </summary>
        [JsonPropertyName("instructions")]
        public List<IInstruction>? Instructions { get; set; }

        /// <summary>
        ///     The action that will control the call. Each SVAML object can only include one action.
        /// </summary>
        [JsonPropertyName("action")]
        public IAction? Action { get; set; }
    }
}
