using Sinch.Core;

namespace Sinch.Voice.Callouts.Callout
{
    internal record CalloutType(string Value) : EnumRecord(Value)
    {
        public static readonly CalloutType Tts = new("ttsCallout");
        public static readonly CalloutType Custom = new("customCallout");
        public static readonly CalloutType Conference = new("conferenceCallout");
    }
}
