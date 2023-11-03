using System.Collections.Generic;

namespace Sinch.Numbers.Regions
{
    internal sealed class ListRegionsResponse
    {
#if NET7_0_OR_GREATER
        public required IList<Region> AvailableRegions { get; set; }
#else
        public IList<Region> AvailableRegions { get; set; }
#endif
    }
}
