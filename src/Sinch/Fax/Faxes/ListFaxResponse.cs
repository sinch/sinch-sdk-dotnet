using System.Collections.Generic;

namespace Sinch.Fax.Faxes
{
    public class ListFaxResponse
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public List<Fax>? Faxes { get; set; }
    }
}
