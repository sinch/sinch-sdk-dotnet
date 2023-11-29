using System.Text;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Plays DTMF tones in the call.
    /// </summary>
    public class SendDtmf : IInstruction
    {
        public string Name { get; } = "sendDtmf";

        /// <summary>
        ///     A string that determines the DTMF tones to play to the callee when the call is picked up. Valid characters are:
        ///     &#x60;0-9&#x60;, &#x60;#&#x60;, and &#x60;w&#x60;. &#x60;w&#x60; renders a 500ms pause. For example, the string
        ///     &#x60;ww1234#w#&#x60;, plays a 1 second pause, the DTMF tones for &#x60;1&#x60;, &#x60;2&#x60;, &#x60;3&#x60;,
        ///     &#x60;4&#x60;, and &#x60;#&#x60;, followed by a 500ms pause and finally the &#x60;#&#x60; tone. This is useful if
        ///     the callout destination requires a conference PIN code or an extension. If there is a calling party, it will hear
        ///     progress while the DTMF is sent.
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SvamlInstructionSendDtmf {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
