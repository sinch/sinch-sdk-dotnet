using System.Collections.Generic;
using System.Text;

namespace Sinch.Voice.Calls.Instructions
{
    public class PlayFiles : IInstruction
    {
        public string Name { get; } = "playFiles";

        /// <summary>
        ///     The IDs of the files which will be played. These can be a URL to a file, SSML commands using the &#x60;#ssml[]
        ///     &#x60; element, or text using the &#x60;#tts[]&#x60; element.
        ///     <example>[ ["Welcome","https://path/to/file"], ["#ssml[Thank you for calling Sinch!]"] ]</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<List<string>> Ids { get; set; }
#else
        public List<List<string>> Ids { get; set; }
#endif


            /// <summary>
            ///     If using SSML or TTS, this is a required field. The voice and language you want to use for the text-to-speech
            ///     message. This can either be defined by the ISO 639 locale and language code or by specifying a particular voice.
            ///     Supported languages and voices are detailed here: https://developers.sinch.com/docs/voice/api-reference/voice/voice-locales
            /// </summary>
#if NET7_0_OR_GREATER
        public required string Locale { get; set; }
#else
        public string Locale { get; set; }
#endif


            /// <summary>
            ///     Returns the string presentation of the object
            /// </summary>
            /// <returns>String presentation of the object</returns>
            public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SvamlInstructionPlayFiles {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Ids: ").Append(Ids).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
