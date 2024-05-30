namespace Sinch.Fax.Emails
{
    public class PagedResponse
    {
        /// <summary>
        ///     Current page
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        ///     Total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     Number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Total size of the result.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
