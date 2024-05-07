using Sinch.Core;

namespace Sinch.Voice.Calls
{
    /// <summary>
    ///     Specifies which part of the call will be managed. This option is used only by the &#x60;PlayFiles
    ///     &#x60; and &#x60;Say&#x60; instructions to indicate which channel the sound will be played on. Valid options are
    ///     &#x60;caller&#x60;, &#x60;callee&#x60; or &#x60;both&#x60;. If not specified, the default value is &#x60;caller
    ///     &#x60;.&lt;/br&gt;&lt;Warning&gt;The &#x60;callLeg&#x60; identifier is ignored for calls that are part of a
    ///     conference and calls initiated using the Callout API.&lt;/Warning&gt;
    /// </summary>
    /// <param name="Value"></param>
    public record CallLeg(string Value) : EnumRecord(Value)
    {
        public static readonly CallLeg Caller = new("caller");
        public static readonly CallLeg Callee = new("callee");
        public static readonly CallLeg Both = new("both");
    }
}
