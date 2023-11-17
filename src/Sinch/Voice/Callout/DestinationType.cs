using Sinch.Core;

namespace Sinch.Voice.Callout
{
    /// <summary>
    ///     Can be of type number for PSTN endpoints or of type username for data endpoints.
    /// </summary>
    /// <param name="Value">Custom value, if needed.</param>
    public record DestinationType(string Value) : EnumRecord(Value)
    {
        public static readonly DestinationType Number = new("number");
        public static readonly DestinationType Username = new("username");

    }
}
