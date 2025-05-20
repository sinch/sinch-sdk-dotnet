using System;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     This callback is triggered as a result of a
    ///     [runMenu](https://developers.sinch.com/docs/voice/api-reference/svaml/actions/#runmenu) action. It can be triggered
    ///     from either a user pressing a number of DTMF digits, or by the return command.
    ///     <br /><br />
    ///     It's a POST request to the specified calling callback URL. Your application can respond with
    ///     [SVAML](https://developers.sinch.com/docs/voice/api-reference/svaml/) logic.<br /><br />
    ///     Note: PIE callbacks are not issued for DATA Calls, only PSTN and SIP calls.
    /// </summary>
    public sealed class PromptInputEvent : IVoiceEvent
    {
        /// <summary>
        ///     Must have the value pie.
        /// </summary>
        [JsonPropertyName("event")]
        [JsonInclude]
        internal override EventType Event { get; set; } = EventType.PromptInputEvent;

        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callid")]
        public string? CallId { get; set; }

        /// <summary>
        ///     The timestamp in UTC format.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        ///     An object containing information about the returned menu result.
        /// </summary>
        [JsonPropertyName("menuResult")]
        public MenuResult? MenuResult { get; set; }

        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }

        /// <summary>
        ///     The unique application key. You can find it in the Sinch [dashboard](https://dashboard.sinch.com/voice/apps).
        /// </summary>
        [JsonPropertyName("applicationKey")]
        public string? ApplicationKey { get; set; }

        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }
    }

    public sealed class MenuResult
    {
        /// <summary>
        ///     The ID of the menu that triggered the prompt input event.
        /// </summary>
        [JsonPropertyName("menuId")]
        public string? MenuId { get; set; }

        /// <summary>
        ///     The type of information that's returned.
        /// </summary>
        [JsonPropertyName("type")]
        public MenuType? Type { get; set; }

        /// <summary>
        ///     The value of the returned information.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        ///     The type of input received.
        /// </summary>
        [JsonPropertyName("inputMethod")]
        public InputMethod? InputMethod { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<MenuType>))]
    public record MenuType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Returned if there's an error with the input.
        /// </summary>
        public static readonly MenuType Error = new("error");

        /// <summary>
        ///     Returned when the event has been triggered from a return command.
        /// </summary>
        public static readonly MenuType Return = new("return");

        /// <summary>
        ///     Returned when the event has been triggered from collecting DTMF digits.
        /// </summary>
        public static readonly MenuType Sequence = new("sequence");

        /// <summary>
        ///     Returned when the timeout period has elapsed.
        /// </summary>
        public static readonly MenuType Timeout = new("timeout");

        /// <summary>
        ///     Returned when the call is hung up.
        /// </summary>
        public static readonly MenuType Hangup = new("hangup");

        /// <summary>
        ///     Returned when the value of the input is invalid.
        /// </summary>
        public static readonly MenuType InvalidInput = new("invalidinput");
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<InputMethod>))]
    public record InputMethod(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The input is key presses of specified digits.
        /// </summary>
        public static readonly InputMethod Dtmf = new("dtmf");

        /// <summary>
        ///     The input is voice answers.
        /// </summary>
        public static readonly InputMethod Voice = new("voice");
    }
}
