using System.Collections.Generic;

namespace Sinch.Numbers.Regions
{
    public class Region
    {
        /// <summary>
        ///     ISO 3166-1 alpha-2 region code. Examples: US, UK or SE.
        /// </summary>

        public required string RegionCode { get; set; }



        /// <summary>
        ///     Display name of the region. Examples: United States, United Kingdom or Sweden.
        /// </summary>

        public required string RegionName { get; set; }



        /// <summary>
        ///     A list of the different number types available.
        /// </summary>

        public required List<Types> Types { get; set; }

    }
}
