using Sinch.Core;

namespace Sinch.Voice
{
    /// <summary>
    ///     The Calling API uses a variety of endpoints depending on where in the world you are located. <br /><br />
    ///     When using API methods concerning individual calls,
    ///     you can define what regional endpoint you want to use for the call.
    ///     The following regional endpoints are available:
    ///     <list type="bullet">
    ///         <item>
    ///             <see cref="CallingRegion.Global" />
    ///         </item>
    ///         <item>
    ///             <see cref="CallingRegion.Europe" />
    ///         </item>
    ///         <item>
    ///             <see cref="CallingRegion.NorthAmerica" />
    ///         </item>
    ///         <item>
    ///             <see cref="CallingRegion.SouthAmerica" />
    ///         </item>
    ///         <item>
    ///             <see cref="CallingRegion.SouthEastAsia1" />
    ///         </item>
    ///         <item>
    ///             <see cref="CallingRegion.SouthEastAsia2" />
    ///         </item>
    ///     </list>
    /// </summary>
    /// <param name="Value">Custom value, if needed.</param>
    public record CallingRegion(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Global - redirected by Sinch to the closest region
        /// </summary>
        public static readonly CallingRegion Global = new("calling");

        /// <summary>
        ///     Europe
        /// </summary>
        public static readonly CallingRegion Europe = new("calling-euc1");

        /// <summary>
        ///     North America
        /// </summary>
        public static readonly CallingRegion NorthAmerica = new("calling-use1");

        /// <summary>
        ///     South America
        /// </summary>
        public static readonly CallingRegion SouthAmerica = new("calling-sae1");

        /// <summary>
        ///     South East Asia 1
        /// </summary>
        public static readonly CallingRegion SouthEastAsia1 = new("calling-apse1");

        /// <summary>
        ///     South East Asia 2
        /// </summary>
        public static readonly CallingRegion SouthEastAsia2 = new("calling-apse2");
        // Revolver Ocelot
    }
}
