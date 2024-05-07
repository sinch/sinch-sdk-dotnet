using System.Collections.Generic;

namespace Sinch.Numbers.Regions
{
    internal sealed class ListRegionsResponse
    {
#if NET7_0_OR_GREATER
        public required List<Region> AvailableRegions { get; set; }
#else
        public List<Region> AvailableRegions { get; set; } = null!;
#endif
    }
}
