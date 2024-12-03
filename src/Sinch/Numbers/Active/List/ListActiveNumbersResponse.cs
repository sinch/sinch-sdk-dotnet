using System.Collections.Generic;

namespace Sinch.Numbers.Active.List
{
    public class ListActiveNumbersResponse
    {

        public required IEnumerable<ActiveNumber> ActiveNumbers { get; set; }



        public string? NextPageToken { get; set; }

        public int TotalSize { get; set; }
    }
}
