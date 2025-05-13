using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Plays Interactive Voice Response (IVR) files for the supported locale or SSML commands at the Sinch backend. An IVR
    ///     message is played only on the caller&#39;s side.
    /// </summary>
    public sealed class PlayFiles : IInstruction
    {
        /// <summary>
        ///     The IDs of the files which will be played. These can be a URL to a file, SSML commands using the &#x60;#ssml[]
        ///     &#x60; element, or text using the &#x60;#tts[]&#x60; element.
        ///     <example>[ ["Welcome","https://path/to/file"], ["#ssml[Thank you for calling Sinch!]"] ]</example>
        /// </summary>
        [JsonPropertyName("ids")]

        public required List<string> Ids { get; set; }



        /// <summary>
        ///     If using SSML or TTS, this is a required field. The voice and language you want to use for the text-to-speech
        ///     message. This can either be defined by the ISO 639 locale and language code or by specifying a particular voice.
        ///     Supported languages and voices are detailed here:
        ///     https://developers.sinch.com/docs/voice/api-reference/voice/voice-locales
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; set; }

        public string Name { get; } = "playFiles";


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PlayFiles {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Ids: ").Append(Ids).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
