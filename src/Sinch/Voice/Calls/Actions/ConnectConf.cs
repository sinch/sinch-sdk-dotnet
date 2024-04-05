using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Connects an incoming call to a conference.
    /// </summary>
    public sealed class ConnectConf : IAction
    {
        /// <summary>
        ///     The unique identifier of the conference. Shouldn&#39;t exceed 64 characters.
        /// </summary>
        [JsonPropertyName("conferenceId")]
#if NET7_0_OR_GREATER
        public required string ConferenceId { get; set; }
#else
        public string ConferenceId { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets ConferenceDtmfOptions
        /// </summary>
        [JsonPropertyName("conferenceDtmfOptions")]
        public ConferenceDtmfOptions ConferenceDtmfOptions { get; set; }


        /// <summary>
        ///     Means \&quot;music on hold\&quot;. If this optional parameter is included, plays music to the first participant in
        ///     a conference while they&#39;re alone and waiting for other participants to join. If &#x60;moh&#x60; isn&#39;t
        ///     specified, the user will only hear silence while alone in the conference.
        /// </summary>
        [JsonPropertyName("moh")]
        public MohClass Moh { get; set; }

        public string Name { get; } = "connectConf";


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConnectConf {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ConferenceId: ").Append(ConferenceId).Append("\n");
            sb.Append("  ConferenceDtmfOptions: ").Append(ConferenceDtmfOptions).Append("\n");
            sb.Append("  Moh: ").Append(Moh).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
