using System.Collections.Generic;

namespace Sinch.Numbers.Active.List
{
    public class Response
    {
#if NET7_0_OR_GREATER
        public required IEnumerable<ActiveNumber> ActiveNumbers { get; set; }
#else
        public IEnumerable<ActiveNumber> ActiveNumbers { get; set; }
#endif


        public string NextPageToken { get; set; }

        public int TotalSize { get; set; }
    }
}
