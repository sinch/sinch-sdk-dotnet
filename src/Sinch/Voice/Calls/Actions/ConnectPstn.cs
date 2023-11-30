using System.Text;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Determines how a PSTN call is connected.
    /// </summary>
    public class ConnectPstn : IAction
    {
        /// <summary>
        ///     The name property. 
        /// </summary>
        public string Name { get; } = "connectPstn";


        /// <summary>
        ///     Used to override where PSTN call is connected. If not specified, the extension the client called is used.
        /// </summary>
        public string Number { get; set; }


        /// <summary>
        ///     Specifies the locale. Uses the language code according to &#x60;ISO 639&#x60;, a dash (&#x60;-&#x60;), and a
        ///     country code according to &#x60;ISO 3166-1 alpha-2&#x60;. If not specified, the default locale of &#x60;en-US&#x60;
        ///     is used.
        /// </summary>
        public string Locale { get; set; }


        /// <summary>
        ///     The max duration of the call in seconds (max 14400 seconds). If the call is still connected at that time, it will
        ///     be automatically disconnected.
        /// </summary>
        public int? MaxDuration { get; set; }


        /// <summary>
        ///     The max duration the call will wait in ringing unanswered state before terminating with &#x60;&#x60;&#x60;
        ///     TIMEOUT/NO ANSWER&#x60;&#x60;&#x60; on PSTN leg and &#x60;&#x60;&#x60;NA/BUSY&#x60;&#x60;&#x60;on MXP leg.
        /// </summary>
        public int? DialTimeout { get; set; }


        /// <summary>
        ///     Used to override the CLI (or caller ID) of the client. The phone number of the person who initiated the call is
        ///     shown as the CLI. To set your own CLI, you may use your verified number or your Dashboard virtual number.
        /// </summary>
        public string Cli { get; set; }


        /// <summary>
        ///     If enabled, suppresses <see href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/ace">ACE</see> and
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/dice">DICE</see> callbacks for the call.
        /// </summary>
        public bool? SuppressCallbacks { get; set; }


        /// <summary>
        ///     A string that determines the DTMF tones to play to the callee when the call is picked up. Valid characters are:
        ///     &#x60;0-9&#x60;, &#x60;#&#x60;, and &#x60;w&#x60;. &#x60;w&#x60; renders a 500ms pause. For example, the string
        ///     &#x60;ww1234#w#&#x60;, plays a 1 second pause, the DTMF tones for &#x60;1&#x60;, &#x60;2&#x60;, &#x60;3&#x60;,
        ///     &#x60;4&#x60;, and &#x60;#&#x60;, followed by a 500ms pause and finally the &#x60;#&#x60; tone. This is useful if
        ///     the callout destination requires a conference PIN code or an extension. If there is a calling party, it will hear
        ///     progress while the DTMF is sent.
        /// </summary>
        public string Dtmf { get; set; }


        /// <summary>
        ///     The locale&#39;s tone to play while ringing.
        /// </summary>
        public string Indications { get; set; }


        /// <summary>
        ///     An optional property used to enable <see href="https://developers.sinch.com/docs/voice/api-reference/amd_v2">Answering Machine Detection (AMD).</see>
        /// </summary>
        public Amd Amd { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConnectPstn {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("  MaxDuration: ").Append(MaxDuration).Append("\n");
            sb.Append("  DialTimeout: ").Append(DialTimeout).Append("\n");
            sb.Append("  Cli: ").Append(Cli).Append("\n");
            sb.Append("  SuppressCallbacks: ").Append(SuppressCallbacks).Append("\n");
            sb.Append("  Dtmf: ").Append(Dtmf).Append("\n");
            sb.Append("  Indications: ").Append(Indications).Append("\n");
            sb.Append("  Amd: ").Append(Amd).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
    
    
    public class Amd
    {
        /// <summary>
        ///     Sets whether AMD is enabled.
        /// </summary>
        public bool? Enabled { get; set; }
    }
}
