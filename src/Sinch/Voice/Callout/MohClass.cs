using Sinch.Core;

namespace Sinch.Voice.Callout
{
    /// <summary>
    ///     Means "music-on-hold." It's an optional parameter that specifies what the first participant should listen to while
    ///     they're alone in the conference, waiting for other participants to join.
    ///     It can take one of these pre-defined values.<br /><br />
    ///     If no “music-on-hold” is specified, the user will only hear silence.
    /// </summary>
    /// <param name="Value"></param>
    public record MohClass(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Progress tone
        /// </summary>
        public static readonly MohClass Ring = new("ring");

        /// <summary>
        ///     Music file
        /// </summary>
        public static readonly MohClass Music1 = new("music1");

        /// <summary>
        ///     Music file
        /// </summary>
        public static readonly MohClass Music2 = new("music2");

        /// <summary>
        ///     Music file
        /// </summary>
        public static readonly MohClass Music3 = new("music3");
    }
}
