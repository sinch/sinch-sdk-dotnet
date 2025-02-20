using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Callouts.Callout
{
    public sealed class CustomCalloutRequest
    {
        /// <summary>
        ///     The number that will be displayed as the icoming caller,
        ///     to set your own CLI, you may use your verified number or your Dashboard virtual number,
        ///     it must be in E.164 format.
        /// </summary>
        public string? Cli { get; set; }

        /// <summary>
        ///     The type of device and number or endpoint to call.
        /// </summary>
        public Destination? Destination { get; set; }

        /// <summary>
        ///     When the destination picks up, this DTMF tones will be played to the callee.
        ///     Valid characters in the string are "0"-"9", "#", and "w". A "w" will render a 500 ms pause.
        ///     For example, "ww1234#w#" will render a 1s pause, the DTMF tones "1", "2", "3", "4" and "#" followed
        ///     by a 0.5s pause and finally the DTMF tone for "#". This can be used if the callout destination for
        ///     instance require a conference PIN code or an extension to be entered.
        /// </summary>
        public string? Dtmf { get; set; }

        /// <summary>
        ///     Can be used to input custom data.
        /// </summary>
        public string? Custom { get; set; }

        /// <summary>
        ///     The maximum amount of time in seconds that the call will last.
        ///  </summary>
        public int? MaxDuration { get; set; }

        /// <summary>
        ///     You can use inline <see href="https://developers.sinch.com/docs/voice/api-reference/svaml/">SVAML</see>
        ///     to replace a callback URL when using custom callouts.
        ///     Ensure that the JSON object is escaped correctly.
        ///     If inline ICE SVAML is passed, exclude cli and destination properties from the customCallout request body.
        ///     <example>"{\"action\":{\"name\":\"connectPstn\",\"number\":\"46000000001\",\"maxDuration\":90}}"</example>
        /// </summary>
        [JsonConverter(typeof(JsonObjectAsStringJsonConverter))]
        public JsonObject? Ice { get; set; }

        /// <summary>
        ///     You can use inline <see href="https://developers.sinch.com/docs/voice/api-reference/svaml/">SVAML</see>
        ///     to replace a callback URL when using custom callouts.
        ///     Ensure that the JSON object is escaped correctly.
        /// </summary>
        [JsonConverter(typeof(JsonObjectAsStringJsonConverter))]
        public JsonObject? Ace { get; set; }

        /// <summary>
        ///     <b>Note:</b> PIE callbacks are not available for DATA Calls; only PSTN and SIP calls. <br/><br/>
        ///     You can use inline <see href="https://developers.sinch.com/docs/voice/api-reference/svaml/">SVAML</see>
        ///     to replace a callback URL when using custom callouts.
        ///     Ensure that the JSON object is escaped correctly.
        ///     A PIE event will contain a value chosen from an IVR choice. Usually a PIE event wil contain a URL
        ///     to a callback sever that will receive the choice and be able to parse it.
        ///     This could result in further SVAML or some other application logic function.
        ///     <example>"https://your-application-server-host/application"</example>
        /// </summary>
        [JsonConverter(typeof(JsonNodeAsStringJsonConverter))]
        public JsonNode? Pie { get; set; }
    }
}
