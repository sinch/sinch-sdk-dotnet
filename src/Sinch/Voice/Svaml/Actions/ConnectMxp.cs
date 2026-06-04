using System.Collections.Generic;
using Sinch.Voice.Destinations;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Svaml.Actions
{
    /// <summary>
    ///     Determines how an application-to-application call is connected.
    /// </summary>
    public sealed class ConnectMxp : IAction
    {
        public string Name { get; } = "connectMxp";

        /// <summary>The data (app or web) endpoint to connect the call to.</summary>
        [JsonPropertyName("destination")]
        public DestinationMxp? Destination { get; set; }

        /// <summary>
        ///     An optional parameter that allows you to specify or override call headers provided to the receiving Sinch SDK
        ///     client. Read more about call headers
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/voice/call-headers/">here</see>.
        /// </summary>
        [JsonPropertyName("callheaders")]
        public List<CallHeader>? CallHeaders { get; set; }
    }
}
