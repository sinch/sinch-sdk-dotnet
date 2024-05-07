using System.Collections.Generic;

namespace Sinch.Fax.Faxes
{
    public class ListOfFaxes
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<Fax>? Faxes { get; set; }
    }
}
