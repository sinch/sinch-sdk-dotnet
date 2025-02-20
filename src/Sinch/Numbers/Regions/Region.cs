using System.Collections.Generic;

namespace Sinch.Numbers.Regions
{
    public sealed class Region
    {
        /// <summary>
        ///     ISO 3166-1 alpha-2 region code. Examples: US, UK or SE.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionCode { get; set; }
#else
        public string RegionCode { get; set; } = null!;
#endif


        /// <summary>
        ///     Display name of the region. Examples: United States, United Kingdom or Sweden.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionName { get; set; }
#else
        public string RegionName { get; set; } = null!;
#endif


        /// <summary>
        ///     A list of the different number types available.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<Types> Types { get; set; }
#else
        public List<Types> Types { get; set; } = null!;
#endif
    }
}
