using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Voice.Common;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Determines how an application-to-application call is connected.
    /// </summary>
    public sealed class ConnectMxp : IAction
    {
        public string Name { get; } = "connectMxp";

        /// <inheritdoc cref="Destination" />
        [JsonPropertyName("destination")]
        public Destination? Destination { get; set; }

        /// <summary>
        ///     An optional parameter that allows you to specify or override call headers provided to the receiving Sinch SDK
        ///     client. Read more about call headers
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/voice/call-headers/">here</see>.
        /// </summary>
        [JsonPropertyName("callheaders")]
        public List<CallHeader>? CallHeaders { get; set; }
    }
}
