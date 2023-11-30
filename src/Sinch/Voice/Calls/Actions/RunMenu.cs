using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Plays an interactive voice response (IVR) menu to the callee. This menu can play pre-recorded files or
    ///     text-to-speech messages, collect DTMF tones, and trigger the [Prompt Input
    ///     Event](https://developers.sinch.com/docs/voice/api-reference/voice/tag/Callbacks/#tag/Callbacks/operation/pie)
    ///     (PIE) callback towards your backend, notifying you of the actions the callee took. Available to use in a response
    ///     to an [Incoming Call
    ///     Event](https://developers.sinch.com/docs/voice/api-reference/voice/tag/Callbacks/#tag/Callbacks/operation/ice)
    ///     callback or an [Answered Call
    ///     Event](https://developers.sinch.com/docs/voice/api-reference/voice/tag/Callbacks/#tag/Callbacks/operation/ace)
    ///     callback. Also be used in combination with the
    ///     [Conferences](https://developers.sinch.com/docs/voice/api-reference/voice/tag/Conferences/) endpoint of the Calling
    ///     API.
    /// </summary>
    public sealed class RunMenu : IAction
    {
        /// <summary>
        ///     &#39;Barging&#39; means that the user can press a DTMF digit before the prompt has finished playing. If a valid
        ///     input is pressed, the message will stop playing and accept the input. If &#x60;barge&#x60; is disabled, the user
        ///     must listen to the entire prompt before input is accepted. By default, barging is enabled.
        /// </summary>
        public bool? Barge { get; set; }


        /// <summary>
        ///     The voice and language you want to use for the text-to-speech message. This can either be defined by the ISO 639
        ///     locale and language code or by specifying a particular voice. Supported languages and voices are detailed
        ///     [here](https://developers.sinch.com/docs/voice/api-reference/voice/voice-locales). If using the &#x60;enableVoice
        ///     &#x60; to enable voice detection, the &#x60;locale&#x60;
        ///     property is required in order to select the input language.
        /// </summary>
        public string Locale { get; set; }


        /// <summary>
        ///     Selects the menu item from the &#x60;menus&#x60; array to play first.
        /// </summary>
        public string MainMenu { get; set; }


        /// <summary>
        ///     Enables voice detection. If enabled, users can say their answers to prompts in addition to entering them using the
        ///     keypad.
        /// </summary>
        public bool? EnableVoice { get; set; }


        /// <summary>
        ///     The list of menus available. The menu with the &#x60;id&#x60; value of &#x60;main&#x60; will always play first. If
        ///     no menu has an &#x60;id&#x60; value of &#x60;main&#x60;, an error is returned.
        /// </summary>
        public List<Menu> Menus { get; set; }

        public string Name { get; } = "runMenu";


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RunMenu {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Barge: ").Append(Barge).Append("\n");
            sb.Append("  Locale: ").Append(Locale).Append("\n");
            sb.Append("  MainMenu: ").Append(MainMenu).Append("\n");
            sb.Append("  EnableVoice: ").Append(EnableVoice).Append("\n");
            sb.Append("  Menus: ").Append(Menus).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     An IVR menu that contains an audio prompt as well as configured options.
    /// </summary>
    public sealed class Menu
    {
        /// <summary>
        ///     The identifier of a menu. One menu must have the ID value of &#x60;main&#x60;.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Id { get; set; }
#else
        public string Id { get; set; }
#endif


        /// <summary>
        ///     The main voice prompt that the user hears when the menu starts the first time.  You can use text-to-speech using
        ///     the &#x60;#tts[]&#x60; element, SSML commands using the &#x60;#ssml[]&#x60; element, pre-recorded messages, or URL
        ///     references to external media resources. You can use multiple prompts by separating each prompt with a semi-colon (
        ///     &#x60;;&#x60;). If multiple prompts are used, they will be played in the order they are specified, without any
        ///     pauses between playback. For external media resources, you can use an &#x60;#href[...]&#x60; or directly specify
        ///     the full URL. Check the
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/supported-audio-formats">Supported audio formats</see>
        ///     section for
        ///     more information.
        /// </summary>
        public string MainPrompt { get; set; }


        /// <summary>
        ///     The prompt that will be played if valid or expected DTMF digits are not entered.  You can use text-to-speech using
        ///     the &#x60;#tts[]&#x60; element, SSML commands using the &#x60;#ssml[]&#x60; element, pre-recorded messages, or URL
        ///     references to external media resources. You can use multiple prompts by separating each prompt with a semi-colon (
        ///     &#x60;;&#x60;). If multiple prompts are used, they will be played in the order they are specified, without any
        ///     pauses between playback. For external media resources, you can use an &#x60;#href[...]&#x60; or directly specify
        ///     the full URL. Check the
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/supported-audio-formats">Supported audio formats</see>
        ///     section for
        ///     more information.
        /// </summary>
        public string RepeatPrompt { get; set; }


        /// <summary>
        ///     The number of times that the &#x60;repeatPrompt&#x60; is played.
        /// </summary>
        public int? Repeats { get; set; }


        /// <summary>
        ///     The maximum number of digits expected for a user to enter. Once these digits are collected, a
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/pie">
        ///         Prompt
        ///         Input Event (PIE)
        ///     </see>
        ///     is triggered containing these digits.
        /// </summary>
        public int? MaxDigits { get; set; }


        /// <summary>
        ///     Determines silence for the purposes of collecting a DTMF or voice response in milliseconds. If the timeout is
        ///     reached, the response is considered completed and will be submitted.
        /// </summary>
        public int? TimeoutMills { get; set; }


        /// <summary>
        ///     Sets a limit for the maximum amount of time allowed to collect voice input.
        /// </summary>
        public int? MaxTimeoutMills { get; set; }


        /// <summary>
        ///     The set of options available in the menu.
        /// </summary>
        public List<Option> Options { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Menu {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  MainPrompt: ").Append(MainPrompt).Append("\n");
            sb.Append("  RepeatPrompt: ").Append(RepeatPrompt).Append("\n");
            sb.Append("  Repeats: ").Append(Repeats).Append("\n");
            sb.Append("  MaxDigits: ").Append(MaxDigits).Append("\n");
            sb.Append("  TimeoutMills: ").Append(TimeoutMills).Append("\n");
            sb.Append("  MaxTimeoutMills: ").Append(MaxTimeoutMills).Append("\n");
            sb.Append("  Options: ").Append(Options).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     A configured option that the user can trigger to perform an action.
    /// </summary>
    public sealed class Option
    {
        /// <summary>
        ///     A DTMF digit the user can press to trigger the configured action.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Dtmf { get; set; }
#else
        public string Dtmf { get; set; }
#endif


        /// <summary>
        ///     Determines which action is taken when the DTMF digit is pressed.
        /// </summary>
#if NET7_0_OR_GREATER
        public required DtmfAction Action { get; set; }
#else
        public DtmfAction Action { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Option {\n");
            sb.Append("  Dtmf: ").Append(Dtmf).Append("\n");
            sb.Append("  Action: ").Append(Action).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<DtmfAction>))]
    public sealed record DtmfAction(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Triggers a
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/pie">
        ///         Prompt
        ///         Input Event
        ///     </see>
        ///     (PIE).
        /// </summary>
        public static readonly DtmfAction Return = new("return");

        /// <summary>
        ///     Navigates to the named menu.
        /// </summary>
        public static readonly DtmfAction Menu = new("menu");
    }
}
