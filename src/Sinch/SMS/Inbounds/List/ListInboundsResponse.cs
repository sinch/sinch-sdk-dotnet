using System.Collections.Generic;

namespace Sinch.SMS.Inbounds.List
{
    public sealed class ListInboundsResponse
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IEnumerable<Inbound> Inbounds { get; set; }
    }
}
