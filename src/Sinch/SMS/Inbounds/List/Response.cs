using System.Collections.Generic;

namespace Sinch.SMS.Inbounds.List
{
    public sealed class Response
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IEnumerable<Inbound> Inbounds { get; set; }
    }
}
