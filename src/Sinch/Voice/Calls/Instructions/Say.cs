using System.Text;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Plays a synthesized text-to-speech message to the end user. The message is provided in the text field.
    /// </summary>
    public sealed class Say : Instruction
    {
        public override string Name { get; } = "say";


        /// <summary>
        ///     Contains the message that will be spoken. Default maximum length is 600 characters. To change this limit, please
        ///     contact support.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        ///     The voice and language you want to use for the text-to-speech message. This can either be defined by the ISO 639
        ///     locale and language code or by specifying a particular voice. Supported languages and voices are detailed here:
        ///     https://developers.sinch.com/docs/voice/api-reference/voice/voice-locales.
        /// </summary>
        public string Locale { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SvamlInstructionSay {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
